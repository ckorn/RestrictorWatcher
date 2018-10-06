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

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.lbDisallowedProcesses.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                rw.RunRestrictor((RestrictorWatcher.DisallowedProcess)lbDisallowedProcesses.Items[index]);
            }
        }

        private async void timerRefreshList_Tick(object sender, EventArgs e)
        {
            await AddNewProcesses();
        }

        private async Task AddNewProcesses()
        {
            List<RestrictorWatcher.DisallowedProcess> list = await rw.Run();

            foreach (RestrictorWatcher.DisallowedProcess item in list)
            {
                lbDisallowedProcesses.Items.Add(item);
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await AddNewProcesses();
        }
    }
}
