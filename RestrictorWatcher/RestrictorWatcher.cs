using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestrictorWatcher
{
    public class RestrictorWatcher
    {
        private const string LOGFILE = @"C:\Users\Christoph Korn\AppData\Local\Temp\restrictor.txt";
        private const string RESTRICTOR = @"C:\Users\Christoph Korn\Tools\Restrictor\Restrictor.exe";
        private readonly List<DisallowedProcess> lastDisallowedTasks = new List<DisallowedProcess>();
        private readonly Regex disallowRegex = null;
        private readonly Regex illegalPrefixRegex = null;
        private readonly FileSystemWatcher logfileWatcher = null;
        public delegate void NewProcessesEventHandler(object sender, LogFileChangedEventArgs e);
        public event NewProcessesEventHandler NewProcesses;

        public RestrictorWatcher()
        {
            //chrome.exe (PID = 10404) identified C:\Users\Christoph Korn\AppData\Local\Google\Chrome\User Data\SwReporter\34.174.200\software_reporter_tool.exe as Disallowed using default rule, Guid = {11015445-d282-4f86-96a2-9e485f593302}
            disallowRegex = new Regex(@"^(?<name>.*) \(PID = (?<pid>\d+)\) identified (?<path>.*) as Disallowed",
                RegexOptions.Compiled);
            //\\?\C:\Users\Christoph Korn\AppData\Roaming\discord\0.0.301\modules\discord_hook\14\DiscordHookHelper.exe
            illegalPrefixRegex = new Regex(@"^\\\\\?\\", RegexOptions.Compiled);

            FileInfo f = new FileInfo(LOGFILE);
            logfileWatcher = new FileSystemWatcher(f.DirectoryName, f.Name)
            {
                EnableRaisingEvents = false
            };
            logfileWatcher.Error += LogfileWatcher_Error;
            logfileWatcher.Changed += LogfileWatcher_Changed;
        }

        public async void StartWatching()
        {
            await ScanLogFile();
            logfileWatcher.EnableRaisingEvents = true;
        }

        private async void LogfileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            await ScanLogFile();
        }

        private async Task ScanLogFile()
        {
            List<DisallowedProcess> list = await this.Run();
            if (list.Count > 0)
            {
                OnNewProcesses(list);
            }
        }

        private void LogfileWatcher_Error(object sender, ErrorEventArgs e)
        {
            throw new Exception("LogfileWatcher error", e.GetException());
        }

        public void OnNewProcesses(List<DisallowedProcess> newProcesses)
        {
            NewProcesses?.Invoke(this, new LogFileChangedEventArgs(newProcesses));
        }

        [Benchmark]
        private async Task<List<DisallowedProcess>> Run()
        {
            List<DisallowedProcess> list = new List<DisallowedProcess>();
            string[] currentLines = await Task.Run(() => File.ReadAllLines(LOGFILE));
            int currentLinecount = currentLines.Length;
            list = await Task.Run(() => GetDisallowedProcesses(currentLines));
            list = await Task.Run(() => AddNewDisallowedProcesses(list));
            return list;
        }

        private List<DisallowedProcess> AddNewDisallowedProcesses(List<DisallowedProcess> newList)
        {
            List<DisallowedProcess> newAdded = new List<DisallowedProcess>();

            lock (lastDisallowedTasks)
            {
                if ((newList.Count > 0) && (lastDisallowedTasks.Count > 0))
                {
                    // if the log is emptied the max line should differ.
                    // So all lines coming now are considered new lines
                    int currentMax = lastDisallowedTasks.AsParallel().Max(x => x.Line);
                    int newMax = newList.AsParallel().Max(x => x.Line);

                    if (newMax < currentMax)
                    {
                        lastDisallowedTasks.Clear();
                    }
                }

                foreach (DisallowedProcess process in newList.OrderBy(x => x.Line))
                {
                    if (!this.lastDisallowedTasks.Any(x => x.Line == process.Line))
                    {
                        lastDisallowedTasks.Add(process);
                        newAdded.Add(process);
                    }
                } 
            }

            return newAdded;
        }

        private List<DisallowedProcess> GetDisallowedProcesses(string[] lines)
        {
            List<DisallowedProcess> list = new List<DisallowedProcess>();

            var part = Partitioner.Create(0, lines.Length, 100);
            Parallel.ForEach(part, range =>
            {
                Parallel.For(range.Item1, range.Item2,
                    () => new List<DisallowedProcess>(),
                    (x, state, tls) =>
                    {
                        Match m = disallowRegex.Match(lines[x]);
                        if ((m != null) && (m.Success))
                        {
                            tls.Add(new DisallowedProcess(
                                Int32.Parse(m.Groups["pid"].Value),
                                m.Groups["name"].Value,
                                illegalPrefixRegex.Replace(m.Groups["path"].Value, string.Empty),
                                x + 1));
                        }
                        return tls;
                    },
                    (x) => { lock (list) { list.AddRange(x); } }
                 );
            });

            return list;
        }

        public class DisallowedProcess
        {
            public int Pid { get; private set; }
            public string Name { get; private set; }
            public string Path { get; private set; }
            public int Line { get; private set; }

            public DisallowedProcess(int pid, string name, string path, int line)
            {
                this.Pid = pid;
                this.Name = name;
                this.Path = path;
                this.Line = line;
            }

            public void RunRestrictor()
            {
                string args = $"/NewHash \"{Path}\"";
                Process.Start(RESTRICTOR, args);
            }

            public void OpenFolder()
            {
                FileInfo f = new FileInfo(Path);
                Process.Start(f.DirectoryName);
            }

            public void RunProgram()
            {
                try
                {
                    Process.Start(Path);
                }
                catch (Win32Exception e) when (e.ErrorCode == -2147467259)
                {
                    // Programm wurde geblockt
                }
            }

            public override string ToString()
            {
                return $"pid={Pid} name={Name} path={Path} line={Line}";
            }
        }

        public class LogFileChangedEventArgs : EventArgs
        {
            public List<DisallowedProcess> NewProcesses { get; private set; }

            public LogFileChangedEventArgs(List<DisallowedProcess> newProcesses)
            {
                this.NewProcesses = newProcesses;
            }
        }
    }
}
