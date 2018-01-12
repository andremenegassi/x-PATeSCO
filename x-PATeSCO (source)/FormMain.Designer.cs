namespace CrossPlatformCompatibility
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.newDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareStatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateTestScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDeviceToolStripMenuItem,
            this.compareStatesToolStripMenuItem,
            this.generateTestScriptToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(678, 24);
            this.msMain.TabIndex = 0;
            this.msMain.Text = "menuStrip1";
            // 
            // newDeviceToolStripMenuItem
            // 
            this.newDeviceToolStripMenuItem.Name = "newDeviceToolStripMenuItem";
            this.newDeviceToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.newDeviceToolStripMenuItem.Text = "New Device";
            this.newDeviceToolStripMenuItem.Click += new System.EventHandler(this.newDeviceToolStripMenuItem_Click);
            // 
            // compareStatesToolStripMenuItem
            // 
            this.compareStatesToolStripMenuItem.Name = "compareStatesToolStripMenuItem";
            this.compareStatesToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.compareStatesToolStripMenuItem.Text = "Compare States";
            this.compareStatesToolStripMenuItem.Click += new System.EventHandler(this.compareStatesToolStripMenuItem_Click);
            // 
            // generateTestScriptToolStripMenuItem
            // 
            this.generateTestScriptToolStripMenuItem.Name = "generateTestScriptToolStripMenuItem";
            this.generateTestScriptToolStripMenuItem.Size = new System.Drawing.Size(123, 20);
            this.generateTestScriptToolStripMenuItem.Text = "Generate Test Script";
            this.generateTestScriptToolStripMenuItem.Click += new System.EventHandler(this.generateTestScriptToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 496);
            this.Controls.Add(this.msMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msMain;
            this.Name = "FormMain";
            this.Text = "x-PATeSCO";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem newDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareStatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateTestScriptToolStripMenuItem;
    }
}