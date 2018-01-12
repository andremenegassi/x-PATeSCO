using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrossPlatformCompatibility.Support;
using System.Drawing.Imaging;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System.ComponentModel;

namespace CrossPlatformCompatibility
{
    [TestClass]
    public partial class FormDevice : Form
    {

        AppiumDriver<IWebElement> _driver = null;
        DeviceConfig _deviceConfig = new DeviceConfig();
        
        List<MyNode> _nodes = new List<MyNode>();
        List<MyNode> _nodesSelected = new List<MyNode>();

        string _fileConfigDeviceName;

        public DeviceConfig DeviceConfig
        {
            get { return _deviceConfig; }
        }

        public FormDevice()
        {
            InitializeComponent();
            RegisterLog("[LOG]");
        }

        private void btnConnectionTest_Click(object sender, EventArgs e)
        {
            ConnectionTest(tbServer1IPPorta.Text.Trim());
        }

        private void ConnectionTest(string server)
        {
            try
            {
                server += @"/wd/hub/status";
                var client = new WebClient();
                var text = client.DownloadString(server);
                if (text != "")
                {
                    Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                    if (values["status"].ToString() == "0")
                        RegisterLog("[APPIUM Server] " + server + " - OK");
                    else RegisterLog("[APPIUM Server] " + server + " NOT OK - " + text);
                }
                else RegisterLog("[APPIUM Server] " + server + " NOT OK - " + text);
            }
            catch (Exception ex)
            {
                RegisterLog("[APPIUM Server] " + server + " - " + ex.Message);
            }
        }

        private void GetScreen()
        {

            RegisterLog("[Get Screen] Trying...");

            MyNode.TpGUIScreenshot guiScreenshot = new MyNode.TpGUIScreenshot();
          
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                MemoryStream ms = new MemoryStream(screenshot.AsByteArray);
                guiScreenshot.Screenshot = Image.FromStream(ms);
                guiScreenshot.Width = _driver.Manage().Window.Size.Width;
                guiScreenshot.Height = _driver.Manage().Window.Size.Height;

                RegisterLog("[Get Screen] Finish");
            }
            catch (Exception ex)
            {
                RegisterLog("[Get Screen] Error - " + ex.Message);
            }

            RegisterLog("[Check compatibility] Trying...");

       

            try
            {
                XDocument xml = XDocument.Parse(_driver.PageSource);

                _nodes.Clear();

                List<XElement> nodes = (from n in xml.Descendants() select n).Distinct().ToList();

                int i = 1;

                foreach (XElement item in nodes)
                {
                    MyNode node = new MyNode();
                    node.Platform = (rbServer1PlatAndroid.Checked ? rbServer1PlatAndroid.Text : rbServer1PlatIOS.Text);
                    node.Element = item;
                    node.XMLDoc = xml.ToString();
                    node.GUIScreenshot = guiScreenshot;
                      
                    node.ElementXPathAbsoluteSelector = Support.XMLUtils.GetXPath(item);
                    node.ElementType = item.Name.LocalName;
              
                    node.KeyAttributes = new Dictionary<string, string>();

                    string[] attrsName;

                    if (rbServer1PlatAndroid.Checked)
                        attrsName = MyNode.AndroidKeyAttributes;
                    else attrsName = MyNode.IOSKeyAttributes;

                    foreach (string attrName in attrsName)
                    {
                        if (item.Attribute(attrName) != null && item.Attribute(attrName).Value != "")
                            node.KeyAttributes.Add(attrName, item.Attribute(attrName).Value);
                    }



                    ////xxxxxxxxxxxxxxxxxxxx.....
                    //List<string> xPathAttrs = new List<string>();
                    //foreach (var attr in node.KeyAttributes)
                    //{
                    //    xPathAttrs.Add("@" + attr.Key + "='" + attr.Value + "'");
                    //}

                    //node.ElementXPathSelectors = new List<string>();
                    //if (node.KeyAttributes.Count > 0)
                    //{
                    //    //based type and key attributes
                    //    string aux = "//*/" + node.ElementType;
                    //    if (xPathAttrs.Count > 0)
                    //    {
                    //        aux += "[" + string.Join(" and ", xPathAttrs.ToArray()) + "]";
                    //    }
                    //    node.ElementXPathSelectors.Add(aux);


                    //   //based only key attributes
                    //   if (xPathAttrs.Count > 0)
                    //    node.ElementXPathSelectors.Add("//*[" + string.Join(" and ", xPathAttrs.ToArray()) + "]");
                    //}



                    float x, y, w, h;
                    MyNode.ExtractElementDimension(node, out x, out y, out w, out h);
                    node.ElementPositionX = x;
                    node.ElementPositionY = y;
                    node.ElementWidth = w;
                    node.ElementHeight = h;

                   // MyNode.ExtractSelectors(node, null);

                    _nodes.Add(node);
                    i++;
                }


                dgNodes.DataSource = _nodes.Select(o => new { Element = o.Element}).ToList();

                dgNodes.Refresh();

                RegisterLog("[Check compatibility] Finish");
            }
            catch (Exception ex)
            {
                RegisterLog("[Check compatibility] Error - " + ex.Message);
            }

        }

        private void ShowScreenShot(MyNode node)
        {

            Image img = null;

            if (node.Platform == "Android")
            {
                if (rbScreenshot.Checked)
                    img = (Image)node.GUIScreenshot.Screenshot.Clone();
                else if (rbScreenshotAfterExec.Checked)
                {
                    try
                    {
                        img = (Image)node.ExecutationState.GUIScreenshot.Screenshot.Clone();
                    }
                    catch {
                    }

                }
            }
            else //iOS
            {
                if (rbScreenshot.Checked)
                    img = Support.ImageUtils.Scale(node.GUIScreenshot.Screenshot, node.GUIScreenshot.Width, node.GUIScreenshot.Height);
                else if (rbScreenshotAfterExec.Checked)
                    try
                    {
                        img = Support.ImageUtils.Scale(node.ExecutationState.GUIScreenshot.Screenshot, node.GUIScreenshot.Width, node.GUIScreenshot.Height);
                    }
                    catch {

                    }
            }

            if (node != null && img  != null && rbScreenshot.Checked)
            {
                #region Extract element position


                float x = 0, y = 0, w = 0, h = 0;

                x = node.ElementPositionX;
                y = node.ElementPositionY;
                w = node.ElementWidth;
                h = node.ElementHeight;

                Pen pen = new Pen(Color.Red, 7);

                pen.Color = Color.Green;

                #endregion

                //drawn rectangle element in image screenshot
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.DrawRectangle(pen, x, y, w, h);
                }
            }

            if (img != null)
            {
                screen.Image = Support.ImageUtils.ScaleProportionally(img, screen.Width, screen.Height);
            }
            else
            {
                screen.Image = null;
            }
            screen.Invalidate();
        }

        private void RegisterLog(string text)
        {
            log.AppendText("\n\n" + "(" + DateTime.Now.ToString("HH:mm:ss") + ")\n");
            log.AppendText(text + "\n");
            log.SelectionStart = log.Text.Length;
            log.ScrollToCaret();
            log.Refresh();
        }



        private void Connect()
        {

            _deviceConfig.PlatformName = rbServer1PlatAndroid.Checked ? rbServer1PlatAndroid.Text : rbServer1PlatIOS.Text;
            _deviceConfig.PlatformVersion = server1platformVersion.Text;
            _deviceConfig.DeviceName = server1deviceName.Text.Trim();
            _deviceConfig.AppPackage = server1appPackage.Text.Trim();
            _deviceConfig.Server = tbServer1IPPorta.Text;
            _deviceConfig.UUID = server1udid.Text.Trim();
            _deviceConfig.AppName = server1AppName.Text;
            _deviceConfig.AppPath = server1AppPath.Text;
            _deviceConfig.AppActivity = server1AppActivity.Text;

            _deviceConfig.TestCaseName = server1TestCaseName.Text;


            tbNodeDetails.Clear();

            RegisterLog("[Platform] " + _deviceConfig.PlatformName);
            RegisterLog("[Connection] Trying connect...");

            try
            {
                DesiredCapabilities capabilities = new DesiredCapabilities();
                capabilities.SetCapability("platformName", _deviceConfig.PlatformName);
                capabilities.SetCapability("platformVersion", _deviceConfig.PlatformVersion);
                capabilities.SetCapability("deviceName", _deviceConfig.DeviceName);
                capabilities.SetCapability("appPackage", _deviceConfig.AppPackage);
                capabilities.SetCapability("app", _deviceConfig.AppPath);
                capabilities.SetCapability("appActivity", _deviceConfig.AppActivity);

                capabilities.SetCapability("newCommandTimeout", "3000");
                capabilities.SetCapability("sessionOverride", "true");
                capabilities.SetCapability("takesScreenshot", "true");
                capabilities.SetCapability("screenshotWaitTimeout", "3000");

                

                Uri defaultUri = new Uri(_deviceConfig.Server + @"/wd/hub");
                

                if (rbServer1PlatAndroid.Checked)
                    _driver = new AndroidDriver<IWebElement>(defaultUri, capabilities, TimeSpan.FromSeconds(3000));
                else
                {
                    capabilities.SetCapability("automationName", "XCUITest");
                    capabilities.SetCapability("bundleId", _deviceConfig.AppPackage);
                    capabilities.SetCapability("udid", _deviceConfig.UUID);
                    _driver = new IOSDriver<IWebElement>(defaultUri, capabilities, TimeSpan.FromSeconds(3000));
                }
                RegisterLog("[Connection] OK");
            }
            catch (Exception ex)
            {

                RegisterLog("[Connection] ERROR: " + ex.Message);
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {

            SetTitleWindow();
            Connect();

        }

        private void SetTitleWindow()
        {
            this.Text = MyNode.ExtractDeviceId(_deviceConfig);
        }



        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                _driver.Quit();
                RegisterLog("[Disconnect] Finish");
            }
            catch (Exception ex)
            {
                RegisterLog("[Disconnect] ERROR: " + ex.Message);
            }
        }


        private void btnClearConfig_Click(object sender, EventArgs e)
        {
            rbServer1PlatAndroid.Checked = true;
            server1platformVersion.Text = "";
            server1deviceName.Text = "";
            server1appPackage.Text = "";
            tbServer1IPPorta.Text = "";
            server1udid.Text = "";
            server1AppName.Text = "";
            server1AppPath.Text = "";
            server1TestCaseName.Text = "";
        }


        private void lbNodes_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                e.DrawBackground();
                Brush myBrush = Brushes.Black;

                if (_nodes[e.Index].KeyAttributes.Count > 0)
                    myBrush = Brushes.Green;
                else myBrush = Brushes.Red;

                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        private void btnServer1SaveConfig_Click(object sender, EventArgs e)
        {

            saveFileDialog.FileName = _fileConfigDeviceName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
            {
                _deviceConfig.PlatformName = rbServer1PlatAndroid.Checked ? rbServer1PlatAndroid.Text : rbServer1PlatIOS.Text;
                _deviceConfig.PlatformVersion = server1platformVersion.Text;
                _deviceConfig.DeviceName = server1deviceName.Text.Trim();
                _deviceConfig.AppPackage = server1appPackage.Text.Trim();
                _deviceConfig.TakesScreenshot = "on";
                _deviceConfig.SessionOverride = "true";
                _deviceConfig.Server = tbServer1IPPorta.Text;
                _deviceConfig.UUID = server1udid.Text.Trim();
                _deviceConfig.AppName = server1AppName.Text;
                _deviceConfig.AppPath = server1AppPath.Text;
                _deviceConfig.AppActivity = server1AppActivity.Text;

                _deviceConfig.TestCaseName = server1TestCaseName.Text;

                if (File.Exists(saveFileDialog.FileName))
                    File.Delete(saveFileDialog.FileName);

                TextWriter tw = new StreamWriter(saveFileDialog.FileName, true);
                tw.WriteLine(JsonConvert.SerializeObject(_deviceConfig));
                tw.Close();

            }

            SetTitleWindow();
        }

        private void btnServer1OpenConfig_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileName != "")
            {
                TextReader tr = new StreamReader(openFileDialog.FileName);

                _fileConfigDeviceName = openFileDialog.FileName;

                try
                {

                    _deviceConfig = (DeviceConfig)JsonConvert.DeserializeObject<DeviceConfig>(tr.ReadToEnd());

                    if (_deviceConfig.PlatformName == "Android")
                        rbServer1PlatAndroid.Checked = true;
                    else rbServer1PlatIOS.Checked = true;
                    server1platformVersion.Text = _deviceConfig.PlatformVersion;
                    server1deviceName.Text = _deviceConfig.DeviceName;
                    server1appPackage.Text = _deviceConfig.AppPackage;
                    tbServer1IPPorta.Text = _deviceConfig.Server;
                    server1udid.Text = _deviceConfig.UUID;
                    server1AppName.Text = _deviceConfig.AppName;
                    server1AppPath.Text = _deviceConfig.AppPath;
                    server1AppActivity.Text = _deviceConfig.AppActivity;
                    server1TestCaseName.Text = _deviceConfig.TestCaseName;


                    if (_deviceConfig.Events == null)
                    {
                        _deviceConfig.Events = new List<MyNode>();
                    }

                    _nodesSelected = _deviceConfig.Events;

                    

                    #region Reprocess RunTime total for all nodes/events

                    //double runTimeTotal = 0;
                    //foreach (MyNode nAux in _deviceConfig.Events)
                    //{
                    //    runTimeTotal += nAux.ExecutationState.Runtime;
                    //}

                    ////update RuntimePercent
                    //foreach (MyNode nAux in _deviceConfig.Events)
                    //{
                    //    nAux.ExecutationState.RuntimePercent = (100 * nAux.ExecutationState.Runtime) / runTimeTotal;
                    //}


                    #endregion*/

                    //temp


                    showSelectedNodes();


                    SetTitleWindow();
                }
                catch (Exception ex)
                {
                    RegisterLog("[Open File Config] " + ex.Message);
                }

                tr.Close();
            }

        }

        private void btnGetScreen_Click_1(object sender, EventArgs e)
        {
            GetScreen();
        }

     
        private void dgNodes_SelectionChanged(object sender, EventArgs e)
        {
            tbKeyAttributes.Text = "";
            tbNodeDetails.Text = "";

            if (dgNodes.SelectedRows.Count > 0)
            {
                int rowIndex = dgNodes.SelectedRows[0].Index;

                MyNode node = _nodes[rowIndex];

                tbNodeDetails.Text = node.Element.ToString();

                if (node.KeyAttributes.Count > 0)
                {
                    tbKeyAttributes.ForeColor = Color.Green;
                    tbNodeDetails.ForeColor = Color.Green;
                    foreach (var k in node.KeyAttributes)
                    {
                        tbKeyAttributes.Text = k.Key + "=" + "\"" + k.Value + "\"" + Environment.NewLine;
                    }
                }
                else
                {
                    tbKeyAttributes.ForeColor = Color.Red;
                    tbNodeDetails.ForeColor = Color.Red;
                    tbKeyAttributes.Text = "No attributes";
                }


                showSelectedNodesProperties(MyNode.ExtractSelectors(node));


                ShowScreenShot(node);

            }
        }

        private void gridNodes_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridView dg = (DataGridView)sender;

                MyNode node = null;

                if (dg == dgNodes)
                    node = _nodes[e.RowIndex];
                else node = _nodesSelected[e.RowIndex];
    
                if (node.KeyAttributes != null && node.KeyAttributes.Count > 0)
                {
                    dg.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Green;
                    dg.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Green;
                }
                else
                {
                    dg.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    dg.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Red;
                }
          
                dg.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGray;

            }

        }

        private void gridNodes_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dg = (DataGridView)sender;

            using (SolidBrush b = new SolidBrush(dg.RowHeadersDefaultCellStyle.ForeColor))
            {
                string label = "";
                if (sender.Equals(dgNodesSelected))
                {
                    label = (e.RowIndex + 1).ToString();
                }
                else
                {
                    label = (e.RowIndex + 1).ToString();
                }
                e.Graphics.DrawString(label, e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void showSelectedNodes(int selectedIndex = 0)
        {
            dgNodesSelected.DataSource = _nodesSelected.Select(o => new {
                EventName = o.EventName,
                SendClick = o.SendClick,
                SendKeys = o.SendKeys,
                SendKeysText = o.SendKeysText,
                Wait = o.WaitElementBySecond

            }).ToList();

            if (dgNodesSelected.Rows.Count > 0)
                dgNodesSelected.Rows[selectedIndex].Selected = true;

            dgNodesSelected.Refresh();
        }

        private void BtnRunTest_Click(object sender, EventArgs e)
        {
            RegisterLog("[Run Test] Initializing ...");

            try
            {

                this.Cursor = Cursors.WaitCursor;

                int i = 0;
                foreach (var n in _nodesSelected)
                {
                    dgNodesSelected.Rows[i].Selected = true;
                    this.Refresh();
                    i++;
                    RunTest(n, _deviceConfig);
                }

                RegisterLog("[Run Test] Finish...");

            }
            catch (Exception ex)
            {
                RegisterLog("[Run Test] Error - " + ex.Message);
            }

            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnRunTestSelectecEvent_Click(object sender, EventArgs e)
        {
            RegisterLog("[Run Test] Initializing ...");

            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (dgNodesSelected.SelectedRows.Count > 0)
                {
                    MyNode node = _nodesSelected[dgNodesSelected.SelectedRows[0].Index];
                    RunTest(node, _deviceConfig);
                }
            }
            catch (Exception ex)
            {
                RegisterLog("[Run Test] Error - " + ex.Message);

            }
            finally {
                this.Cursor = Cursors.Default;
            }
        }

        private void RunTest(MyNode n, DeviceConfig deviceConfig)
        {
            IWebElement element = null;

            #region update screenshot
            MyNode.TpGUIScreenshot guiScreenshot = new MyNode.TpGUIScreenshot();

            Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            MemoryStream ms = new MemoryStream(screenshot.AsByteArray);
            guiScreenshot.Screenshot = Image.FromStream(ms);
            guiScreenshot.Width = _driver.Manage().Window.Size.Width;
            guiScreenshot.Height = _driver.Manage().Window.Size.Height;

            n.GUIScreenshot = guiScreenshot;
            #endregion

            DateTime timeStart = DateTime.Now;

            if (n.WaitElementBySecond > 0)
                System.Threading.Thread.Sleep(n.WaitElementBySecond * 1000);

            List<XPathSelector> xpaths = new List<XPathSelector>();

            xpaths = MyNode.ExtractSelectors(n);
       
            
            xpaths = MyNode.OrderBySelectorInOrder(xpaths);

            foreach (XPathSelector selector in xpaths)
            {
                //if (selector.Type != XPathSelector.XPathType.AncestorIndex)
                //    continue;

                try
                {
                    if (n.Platform == "Android" && selector.XPathForAndroid != "")
                        element = _driver.FindElementByXPath(selector.XPathForAndroid);
                    else if (n.Platform == "iOS" && selector.XPathForIOS != "")
                        element = _driver.FindElementByXPath(selector.XPathForIOS);
                }
                catch
                {

                    continue;
                }

                if (element != null)
                    break;
            }


            if (element != null)
            {

                try
                {
                    if (n.SendClick)
                    {
                
                        element.Click();
                    }
                    else if (n.SendKeys)
                    {
                        element.Click();
                        element.Clear();
                        element.SendKeys(n.SendKeysText);

                        if (n.Platform == "Android")
                            _driver.HideKeyboard();
                        else if (n.Platform == "iOS")
                            _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
                    }
                }
                catch (Exception ex)
                {
                    RegisterLog("[Run Test] Error - " + ex.Message);
                }

                TimeSpan ts = DateTime.Now.Subtract(timeStart);


                #region Extract status of executation

                //Screenshot screenshotExec = ((ITakesScreenshot)_driver).GetScreenshot();
                //MemoryStream msExec = new MemoryStream(screenshotExec.AsByteArray);

                //MyNode.TpGUIScreenshot guiScreenshotExec = new MyNode.TpGUIScreenshot()
                //{
                //    Screenshot = Image.FromStream(msExec),
                //    Width = _driver.Manage().Window.Size.Width,
                //    Height = _driver.Manage().Window.Size.Height
                //};

                //XDocument xml = XDocument.Parse(_driver.PageSource);

                //n.ExecutationState = new MyNode.TpExecutationState()
                //{
                //    Executed = true,
                //    GUIScreenshot = guiScreenshotExec,
                //    Xml = xml,
                //    Runtime = ts.TotalSeconds
                //};

                #region Reprocess RunTime total for all nodes/events

                double runTimeTotal = 0;
                foreach (MyNode nAux in deviceConfig.Events)
                {
                    runTimeTotal += nAux.ExecutationState.Runtime;
                }

                //update RuntimePercent
                foreach (MyNode nAux in deviceConfig.Events)
                {
                    nAux.ExecutationState.RuntimePercent = (100  * nAux.ExecutationState.Runtime) / runTimeTotal;
                }


                #endregion


                #endregion


            }

        }




        private void dgNodesSelected_SelectionChanged(object sender, EventArgs e)
        {

            if (dgNodesSelected.SelectedRows.Count > 0)
            {
                int rowIndex = dgNodesSelected.SelectedRows[0].Index;
                MyNode node = _nodesSelected[rowIndex];
                ShowScreenShot(node);

                tbScriptEventName.Text = node.EventName;
                rbScriptSendClick.Checked = node.SendClick;
                rbScriptSendKeys.Checked = node.SendKeys;
                tbScriptSendKeysText.Text = node.SendKeysText;
                tbScriptWaitSeconds.Text = node.WaitElementBySecond.ToString();

                showSelectedNodesProperties(MyNode.ExtractSelectors(node));

                tbNodeSelectedDetails.Text = node.Element.ToString();

            }
        }



        private void showSelectedNodesProperties(List<XPathSelector> selectors)
        {
            tbAbsolutePath.Text = "";
            tbIdentifyAttributes.Text = "";
            tbCrossPlatform.Text = "";
            tbElementType.Text = "";
            tbAncestorIndex.Text = "";
            tbAncestorAttributes.Text = "";


            tbAbsolutePath.ForeColor = Color.Black;
            tbIdentifyAttributes.ForeColor = Color.Black;
            tbCrossPlatform.ForeColor = Color.Black;
            tbElementType.ForeColor = Color.Black;
            tbAncestorIndex.ForeColor = Color.Black;
            tbAncestorAttributes.ForeColor = Color.Black;



            if (_deviceConfig.PlatformName == "Android")
            {
                tbAbsolutePath.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.AbsolutePath).DefaultIfEmpty(new XPathSelector()).Single().XPathForAndroid;
                tbIdentifyAttributes.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.IdentifyAttributes).DefaultIfEmpty(new XPathSelector()).Single().XPathForAndroid;
                tbCrossPlatform.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.CrossPlatform).DefaultIfEmpty(new XPathSelector()).Single().XPathForAndroid;
                tbElementType.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.ElementType).DefaultIfEmpty(new XPathSelector()).Single().XPathForAndroid;
                tbAncestorIndex.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.AncestorIndex).DefaultIfEmpty(new XPathSelector()).Single().XPathForAndroid;
                tbAncestorAttributes.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.AncestorAttributes).DefaultIfEmpty(new XPathSelector()).Single().XPathForAndroid;
            }
            else if (_deviceConfig.PlatformName == "iOS")
            {
                tbAbsolutePath.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.AbsolutePath).DefaultIfEmpty(new XPathSelector()).Single().XPathForIOS;
                tbIdentifyAttributes.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.IdentifyAttributes).DefaultIfEmpty(new XPathSelector()).Single().XPathForIOS;
                tbCrossPlatform.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.CrossPlatform).DefaultIfEmpty(new XPathSelector()).Single().XPathForIOS;
                tbElementType.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.ElementType).DefaultIfEmpty(new XPathSelector()).Single().XPathForIOS;
                tbAncestorIndex.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.AncestorIndex).DefaultIfEmpty(new XPathSelector()).Single().XPathForIOS;
                tbAncestorAttributes.Text = selectors.Where(i => i.Type == XPathSelector.XPathType.AncestorAttributes).DefaultIfEmpty(new XPathSelector()).Single().XPathForIOS;
            }

            if (tbAbsolutePath.Text.Trim() == "")
            {
                tbAbsolutePath.ForeColor = Color.Red;
                tbAbsolutePath.Text = "Not Applicable";

            }

            if (tbIdentifyAttributes.Text.Trim() == "")
            {
                tbIdentifyAttributes.ForeColor = Color.Red;
                tbIdentifyAttributes.Text = "Not Applicable";

            }

            if (tbCrossPlatform.Text.Trim() == "")
            {
                tbCrossPlatform.ForeColor = Color.Red;
                tbCrossPlatform.Text = "Not Applicable";

            }

            if (tbElementType.Text.Trim() == "")
            {
                tbElementType.ForeColor = Color.Red;
                tbElementType.Text = "Not Applicable";

            }

            if (tbAncestorIndex.Text.Trim() == "")
            {
                tbAncestorIndex.ForeColor = Color.Red;
                tbAncestorIndex.Text = "Not Applicable";

            }

            if (tbAncestorAttributes.Text.Trim() == "")
            {
                tbAncestorAttributes.ForeColor = Color.Red;
                tbAncestorAttributes.Text = "Not Applicable";

            }

        }

        private void btnNodeSelected_Click(object sender, EventArgs e)
        {
            if (dgNodes.SelectedRows.Count > 0)
            {
                int rowIndex = dgNodes.SelectedRows[0].Index;

                MyNode node = _nodes[rowIndex];

                MyNode nodeSelected = node.Clone();
                _nodesSelected.Add(node);

                dgNodes.Refresh();

                showSelectedNodes();

                if (dgNodesSelected.Rows.Count > 0)
                {
                    dgNodesSelected.Rows[dgNodesSelected.Rows.Count-1].Selected = true;
                }
            }
        }

        private void btnSaveElementConfigTest_Click(object sender, EventArgs e)
        {
            if (dgNodesSelected.SelectedRows.Count > 0)
            {
                int rowIndex = dgNodesSelected.SelectedRows[0].Index;
                MyNode node = _nodesSelected[rowIndex];

                if (tbScriptSendKeysText.Text != "")
                    rbScriptSendKeys.Checked = true;

                node.EventName = tbScriptEventName.Text.Trim();
                node.SendClick = rbScriptSendClick.Checked;
                node.SendKeys = rbScriptSendKeys.Checked;
                node.SendKeysText = tbScriptSendKeysText.Text;

                if (tbScriptWaitSeconds.Text.Trim() != "")
                    node.WaitElementBySecond = int.Parse(tbScriptWaitSeconds.Text);

                showSelectedNodes(rowIndex);

            }
        }

        private void btnNodeSelectedRemove_Click(object sender, EventArgs e)
        {
            if (dgNodesSelected.SelectedRows.Count > 0)
            {
                int rowIndex = dgNodesSelected.SelectedRows[0].Index;

                MyNode node = _nodesSelected[rowIndex];
                _nodesSelected.Remove(node);

                MyNode nodeSelected = _nodes.Find(x => x.Element.ToString().Equals(node.Element.ToString()));

                dgNodes.Refresh();

                showSelectedNodes();
            }
        }

        private void btnNodeSelectedUpDown_Click(object sender, EventArgs e)
        {
            if (dgNodesSelected.SelectedRows.Count == 0)
                return;

            int rowIndex = dgNodesSelected.SelectedRows[0].Index;
            MyNode node = _nodesSelected[rowIndex];

            int newIndex = -1;

            if (sender == btnNodeSelectedUp && rowIndex > 0)
            {
                newIndex = rowIndex - 1;
            }
            else if (sender == btnNodeSelectedDown && rowIndex < _nodesSelected.Count - 1)
            {
                newIndex = rowIndex + 1;
            }

            if (newIndex > -1)
            {
                _nodesSelected.RemoveAt(rowIndex);
                _nodesSelected.Insert(newIndex, node);
                showSelectedNodes(newIndex);
            }
        }



        private void FormDevice_Leave(object sender, EventArgs e)
        {
            //if (_driver != null)
            //    _driver.Close();
        }

        private void FormDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormMain.FormDevices.Remove(this);
        }

        private void rbScreenshot_CheckedChanged(object sender, EventArgs e)
        {
            if (dgNodesSelected.SelectedRows.Count > 0)
            {
                int rowIndex = dgNodesSelected.SelectedRows[0].Index;

                MyNode node = _nodesSelected[rowIndex];
                ShowScreenShot(node);
            }
        }


        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormDevice_Shown(object sender, EventArgs e)
        {
     
            //this.WindowState = FormWindowState.Maximized;
            this.Width = Screen.PrimaryScreen.Bounds.Width - 100;
            this.Height = Screen.PrimaryScreen.Bounds.Height - 100;

            this.HorizontalScroll.Value = 0;
            this.VerticalScroll.Value = 0;

        }

        private void btnSummaryApplicable_Click(object sender, EventArgs e)
        {

            int absolutePath = 0;
            int identifyAttributes = 0;
            int crossPlatform = 0;
            int elementType = 0;
            int ancestorIndex = 0;
            int ancestorAttributes = 0;


            foreach (var node in _nodesSelected)
            {
                List<XPathSelector> selectors = MyNode.ExtractSelectors(node);

                foreach (var s in selectors)
                {

                    string xPath = "";

                    if (_deviceConfig.PlatformName == "Android")
                    {
                        xPath = s.XPathForAndroid;
                    }
                    else if (_deviceConfig.PlatformName == "iOS")
                    {
                        xPath = s.XPathForIOS;
                    }

                    if (s.Type == XPathSelector.XPathType.AbsolutePath && xPath != "")
                        absolutePath++;
                    else if (s.Type == XPathSelector.XPathType.IdentifyAttributes && xPath != "")
                        identifyAttributes++;
                    else if (s.Type == XPathSelector.XPathType.CrossPlatform && xPath != "")
                        crossPlatform++;
                    else if (s.Type == XPathSelector.XPathType.ElementType && xPath != "")
                        elementType++;
                    else if (s.Type == XPathSelector.XPathType.AncestorIndex && xPath != "")
                        ancestorIndex++;
                    else if (s.Type == XPathSelector.XPathType.AncestorAttributes && xPath != "")
                        ancestorAttributes++;

                }

                tbAbsolutePath.Text = "Applicable: " + absolutePath.ToString() + "    NOT Applicable: " + (_nodesSelected.Count - absolutePath).ToString();
                tbIdentifyAttributes.Text = "Applicable: " + identifyAttributes.ToString() + "    NOT Applicable: " + (_nodesSelected.Count - identifyAttributes).ToString();
                tbCrossPlatform.Text = "Applicable: " + crossPlatform.ToString() + "    NOT Applicable: " + (_nodesSelected.Count - crossPlatform).ToString();
                tbElementType.Text = "Applicable: " + elementType.ToString() + "    NOT Applicable: " + (_nodesSelected.Count - elementType).ToString();
                tbAncestorIndex.Text = "Applicable: " + ancestorIndex.ToString() + "    NOT Applicable: " + (_nodesSelected.Count - ancestorIndex).ToString();
                tbAncestorAttributes.Text = "Applicable: " + ancestorAttributes.ToString() + "    NOT Applicable: " + (_nodesSelected.Count - ancestorAttributes).ToString();


                tbAbsolutePath.ForeColor = Color.Blue;
                tbIdentifyAttributes.ForeColor = Color.Blue;
                tbCrossPlatform.ForeColor = Color.Blue;
                tbElementType.ForeColor = Color.Blue;
                tbAncestorIndex.ForeColor = Color.Blue;
                tbAncestorAttributes.ForeColor = Color.Blue;

            }

            

        }
    }

    class ElementSelectedProperties
    {
        [Category("XPath"), ReadOnlyAttribute(true)]
        public string AbsolutePath { get; set; }
        [Category("XPath"), ReadOnlyAttribute(true)]
        public string IdentifyAttributes { get; set; }
        [Category("XPath"), ReadOnlyAttribute(true)]
        public string CrossPlatform { get; set; }
        [Category("XPath"), ReadOnlyAttribute(true)]
        public string ElementType { get; set; }
        [Category("XPath"), ReadOnlyAttribute(true)]
        public string AncestorIndex { get; set; }
        [Category("XPath"), ReadOnlyAttribute(true)]
        public string AncestorAttributes { get; set; }


    }

}
