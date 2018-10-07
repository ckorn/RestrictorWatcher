namespace RestrictorWatcher
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbDisallowedProcesses = new System.Windows.Forms.ListBox();
            this.timerRefreshList = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runRestrictorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripListBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDisallowedProcesses
            // 
            this.lbDisallowedProcesses.ContextMenuStrip = this.contextMenuStripListBox;
            this.lbDisallowedProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDisallowedProcesses.FormattingEnabled = true;
            this.lbDisallowedProcesses.Location = new System.Drawing.Point(0, 0);
            this.lbDisallowedProcesses.Name = "lbDisallowedProcesses";
            this.lbDisallowedProcesses.Size = new System.Drawing.Size(800, 450);
            this.lbDisallowedProcesses.TabIndex = 1;
            this.lbDisallowedProcesses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            this.lbDisallowedProcesses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbDisallowedProcesses_MouseDown);
            // 
            // timerRefreshList
            // 
            this.timerRefreshList.Enabled = true;
            this.timerRefreshList.Interval = 5000;
            this.timerRefreshList.Tick += new System.EventHandler(this.timerRefreshList_Tick);
            // 
            // contextMenuStripListBox
            // 
            this.contextMenuStripListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runRestrictorToolStripMenuItem,
            this.openFolderToolStripMenuItem,
            this.runProgramToolStripMenuItem});
            this.contextMenuStripListBox.Name = "contextMenuStrip1";
            this.contextMenuStripListBox.Size = new System.Drawing.Size(181, 92);
            // 
            // runRestrictorToolStripMenuItem
            // 
            this.runRestrictorToolStripMenuItem.Name = "runRestrictorToolStripMenuItem";
            this.runRestrictorToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.runRestrictorToolStripMenuItem.Text = "Run Restrictor";
            this.runRestrictorToolStripMenuItem.Click += new System.EventHandler(this.runRestrictorToolStripMenuItem_Click);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.openFolderToolStripMenuItem.Text = "Open folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // runProgramToolStripMenuItem
            // 
            this.runProgramToolStripMenuItem.Name = "runProgramToolStripMenuItem";
            this.runProgramToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.runProgramToolStripMenuItem.Text = "Run program";
            this.runProgramToolStripMenuItem.Click += new System.EventHandler(this.runProgramToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbDisallowedProcesses);
            this.Name = "MainForm";
            this.Text = "RestrictorWatcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStripListBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lbDisallowedProcesses;
        private System.Windows.Forms.Timer timerRefreshList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripListBox;
        private System.Windows.Forms.ToolStripMenuItem runRestrictorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runProgramToolStripMenuItem;
    }
}

