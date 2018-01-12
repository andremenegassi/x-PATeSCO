using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrossPlatformCompatibility.Support;

namespace CrossPlatformCompatibility
{
    public partial class FormMain : Form
    {
        public static MyNode NodeDeviceTransfer{ get; set; }
        public static List<FormDevice> FormDevices { get; set; }

        public FormMain()
        {
            InitializeComponent();
            FormDevices = new List<FormDevice>();

        }

        private void newDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDevice f = new FormDevice();
            f.Text = "Device " + (FormDevices.Count + 1);
            FormDevices.Add(f);
            f.MdiParent = this;
            f.Show();
        }

        private void compareStatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCompareStates f = new FormCompareStates();
            f.ShowDialog();
            f = null;
        }

        private void generateTestScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGenerateTestScript f = new FormGenerateTestScript();
            f.ShowDialog();
            f = null;
        }
    }
}
