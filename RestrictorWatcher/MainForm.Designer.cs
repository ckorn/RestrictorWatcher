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
            this.SuspendLayout();
            // 
            // lbDisallowedProcesses
            // 
            this.lbDisallowedProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDisallowedProcesses.FormattingEnabled = true;
            this.lbDisallowedProcesses.Location = new System.Drawing.Point(0, 0);
            this.lbDisallowedProcesses.Name = "lbDisallowedProcesses";
            this.lbDisallowedProcesses.Size = new System.Drawing.Size(800, 450);
            this.lbDisallowedProcesses.TabIndex = 1;
            this.lbDisallowedProcesses.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // timerRefreshList
            // 
            this.timerRefreshList.Enabled = true;
            this.timerRefreshList.Interval = 5000;
            this.timerRefreshList.Tick += new System.EventHandler(this.timerRefreshList_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbDisallowedProcesses);
            this.Name = "Form1";
            this.Text = "RestrictorWatcher";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox lbDisallowedProcesses;
        private System.Windows.Forms.Timer timerRefreshList;
    }
}

