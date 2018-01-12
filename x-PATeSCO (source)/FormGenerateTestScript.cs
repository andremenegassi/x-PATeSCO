using CrossPlatformCompatibility.Support;
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
    public partial class FormGenerateTestScript : Form
    {
        List<int> _cbCRef1Index = new List<int>();
        List<int> _cbCRef2Index = new List<int>();

        public FormGenerateTestScript()
        {
            InitializeComponent();

            List<XPathSelector.XPathType> indExp = new List<XPathSelector.XPathType>();

            foreach (XPathSelector.XPathType item in Enum.GetValues(typeof(XPathSelector.XPathType)))
            {
                if (item != XPathSelector.XPathType.Indefined)
                    indExp.Add(item);
            }

            cbIndividualExp.DataSource = indExp;

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FormGenerateTestScript_Shown(object sender, EventArgs e)
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

 

        private void btnGenerateProject_Click(object sender, EventArgs e)
        {
            ScriptGenerate.EnabledEvaluation = false;
            ScriptGenerate.OnlyTestClass = ckOnlyClass.Checked;
            GenerateProject();

        }

        private void btnGenerateProjectEvaluation_Click(object sender, EventArgs e)
        {
            ScriptGenerate.EnabledEvaluation = true;
            ScriptGenerate.OnlyTestClass = ckOnlyClass.Checked;
            GenerateProject();
           
        }


        private void GenerateProject()
        {

            FormDevice formDevice1 = null;
            FormDevice formDevice2 = null;

            ScriptGenerate.TpLocatorEstrategy locatorEstrategy = ScriptGenerate.TpLocatorEstrategy.IndividualExpression;

            if (rbCombinedExpInOrder.Checked)
                locatorEstrategy = ScriptGenerate.TpLocatorEstrategy.CombinedExpressionsInOrder;
            else if (rbCombinedExpMultiLocator.Checked)
                locatorEstrategy = ScriptGenerate.TpLocatorEstrategy.CombinedExpressionsMultiLocator;



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
                return;
            }

            string msg = "";
            if (!MyNode.CheckDeviceConfigCompatibility(formDevice1.DeviceConfig, formDevice2.DeviceConfig, out msg))
            {
                MessageBox.Show(msg);
                return;
            }


            try
            {
                this.Cursor = Cursors.WaitCursor;

                string dirProject = Support.ScriptGenerate.GenerateVisualStudioProjectTest(formDevice1.DeviceConfig, formDevice2.DeviceConfig, locatorEstrategy, (XPathSelector.XPathType)cbIndividualExp.SelectedItem);
                System.Diagnostics.Process.Start(dirProject);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;

            }
        }

        private void ckOnlyClass_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOnlyClass.Checked)
            {
                btnGenerateProject.Text = "Generate Test Class";
                btnGenerateProjectEvaluation.Text = "Generate Test Class - For Evaluation Approach";
            }
            else
            {
                btnGenerateProject.Text = "Generate Test Project";
                btnGenerateProjectEvaluation.Text = "Generate Test Project- For Evaluation Approach";
            }
        }
    }
}
