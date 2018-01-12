using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossPlatformCompatibility
{
    public partial class FormCompareStates : Form
    {
        List<int> _cbCRef1Index = new List<int>();
        List<int> _cbCRef2Index = new List<int>();

        public FormCompareStates()
        {
            InitializeComponent();
        }

        private void FormCompareStates_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < FormMain.FormDevices.Count; i++)
            {
                if (FormMain.FormDevices[i].Text.ToUpper().Contains("ANDROID"))
                {
                    cbCRef1.Items.Add(FormMain.FormDevices[i].Text);
                    _cbCRef1Index.Add(i);
                }
                else if (FormMain.FormDevices[i].Text.ToUpper().Contains("IOS"))
                {
                    cbCRef2.Items.Add(FormMain.FormDevices[i].Text);
                    _cbCRef2Index.Add(i);
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;

                FormDevice formDevice1 = null;
                FormDevice formDevice2 = null;

                if (cbCRef1.SelectedIndex > -1)
                {
                    formDevice1 = FormMain.FormDevices[_cbCRef1Index[cbCRef1.SelectedIndex]];
                }

                if (cbCRef2.SelectedIndex > -1)
                {
                    formDevice2 = FormMain.FormDevices[_cbCRef2Index[cbCRef2.SelectedIndex]];
                }


                if (formDevice1 == null || formDevice2 == null)
                {
                    MessageBox.Show("Select two devices.");
                }
                else
                {
                    string msg = "";
                    Support.MyNode.CompareStates(formDevice1.DeviceConfig, formDevice2.DeviceConfig, ckTextSimilarity.Checked, ckScreenshotSimilarity.Checked, ckOCRSimilarity.Checked, out msg);

                    if (msg != "")
                    {
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        tbOutput.Text = Support.MyNode.CompareStatesGenerateOutPut(formDevice1.DeviceConfig, formDevice2.DeviceConfig, ckTextSimilarity.Checked, ckScreenshotSimilarity.Checked, ckOCRSimilarity.Checked);
                        MessageBox.Show("Finish.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally {
                this.Cursor = Cursors.Default;
            }
        }

        private void llOpenWebBrowser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo pi = new ProcessStartInfo(tbOutput.Text);
                Process.Start(pi);
            }
            catch (Exception ex) {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
