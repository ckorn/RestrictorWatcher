using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestrictorWatcher
{
    public partial class MainForm : Form
    {
        private RestrictorWatcher rw = new RestrictorWatcher();
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            rw.NewProcesses += Rw_NewProcesses;
            rw.StartWatching();
        }

        private void Rw_NewProcesses(object sender, RestrictorWatcher.LogFileChangedEventArgs e)
        {
            this.Invoke(new Action(() => AddNewProcesses(e.NewProcesses)));
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lbDisallowedProcesses.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                ((RestrictorWatcher.DisallowedProcess)lbDisallowedProcesses.Items[index]).RunRestrictor();
            }
        }

        private void AddNewProcesses(List<RestrictorWatcher.DisallowedProcess> list)
        {
            foreach (RestrictorWatcher.DisallowedProcess item in list)
            {
                lbDisallowedProcesses.Items.Add(item);
            }
            lbDisallowedProcesses.TopIndex = lbDisallowedProcesses.Items.Count - 1;
        }

        private void runRestrictorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSelectedProcess()?.RunRestrictor();
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSelectedProcess()?.OpenFolder();
        }

        private void lbDisallowedProcesses_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Right))
            {
                int index = this.lbDisallowedProcesses.IndexFromPoint(e.Location);
                if (index != System.Windows.Forms.ListBox.NoMatches)
                {
                    lbDisallowedProcesses.SelectedIndex = index;
                }
            }
        }

        private void runProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSelectedProcess()?.RunProgram();
        }

        private RestrictorWatcher.DisallowedProcess GetSelectedProcess()
        {
            RestrictorWatcher.DisallowedProcess ret = null;

            int index = lbDisallowedProcesses.SelectedIndex;
            if (index >= 0)
            {
                ret = (RestrictorWatcher.DisallowedProcess)lbDisallowedProcesses.Items[index];
            }

            return ret;
        }
    }
}
