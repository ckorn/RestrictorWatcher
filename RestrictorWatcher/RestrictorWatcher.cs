﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private string lastChecksum = "";
        private const string LOGFILE = @"C:\Users\Christoph Korn\AppData\Local\Temp\restrictor.txt";
        private const string RESTRICTOR = @"C:\Users\Christoph Korn\Tools\Restrictor\Restrictor.exe";
        private readonly List<DisallowedProcess> lastDisallowedTasks = new List<DisallowedProcess>();
        private readonly Regex disallowRegex = null;

        public RestrictorWatcher()
        {
            //chrome.exe (PID = 10404) identified C:\Users\Christoph Korn\AppData\Local\Google\Chrome\User Data\SwReporter\34.174.200\software_reporter_tool.exe as Disallowed using default rule, Guid = {11015445-d282-4f86-96a2-9e485f593302}
            disallowRegex = new Regex(@"^(?<name>.*) \(PID = (?<pid>\d+)\) identified (?<path>.*) as Disallowed",
                RegexOptions.Compiled);
        }

        private static Task<string> Hash(string input)
        {
            return Task.Run(() =>
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                    var sb = new StringBuilder(hash.Length * 2);

                    foreach (byte b in hash)
                    {
                        // can be "x2" if you want lowercase
                        sb.Append(b.ToString("X2"));
                    }

                    return sb.ToString();
                }
            });
        }

        private static Task<string[]> ReadFile(string file)
        {
            return Task.Run(() =>
            {
                return File.ReadAllLines(file);
            });
        }

        [Benchmark]
        public async Task<List<DisallowedProcess>> Run()
        {
            List<DisallowedProcess> list = new List<DisallowedProcess>();
            string[] currentLines = await ReadFile(LOGFILE);
            string currentContent = await Task.Run(() => string.Join("", currentLines));
            string currentChecksum = await Hash(currentContent);
            if (currentChecksum != lastChecksum)
            {
                list = await GetDisallowedProcesses(currentLines);
                list = await AddNewDisallowedProcesses(list);

                lastChecksum = currentChecksum;
            }
            return list;
        }

        public void RunRestrictor(DisallowedProcess p)
        {
            string args = $"/NewHash \"{p.Path}\"";
            Process.Start(RESTRICTOR, args);
        }

        private Task<List<DisallowedProcess>> AddNewDisallowedProcesses(List<DisallowedProcess> newList)
        {
            return Task.Run(() =>
            {
                List<DisallowedProcess> newAdded = new List<DisallowedProcess>();

                foreach (DisallowedProcess process in newList.OrderBy(x => x.Line))
                {
                    if (!this.lastDisallowedTasks.Any(x => x.Line == process.Line))
                    {
                        lastDisallowedTasks.Add(process);
                        newAdded.Add(process);
                    }
                }

                return newAdded;
            });
        }

        private Task<List<DisallowedProcess>> GetDisallowedProcesses(string[] lines)
        {
            return Task.Run(() =>
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
                                    m.Groups["path"].Value,
                                    x + 1));
                            }
                            return tls;
                        },
                        (x) => { lock (list) { list.AddRange(x); } }
                     );
                });

                return list;
            });
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

            public override string ToString()
            {
                return $"pid={Pid} name={Name} path={Path} line={Line}";
            }
        }
    }
}
