using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using Evaluation;

namespace UnitTestProject.F2
{

    [TestClass]
    public class F2CombinedExpressionsMultiLocator
    {
		AppiumDriver<IWebElement> _driver = null;
		DesiredCapabilities _capabilities = new DesiredCapabilities();
        LocatorStrategy _locator = null; 

		
        [TestMethod]
        public void TestMethodMain()
        {

			/*APPIUM config*/
			_capabilities.SetCapability("platformName", ProjectConfig.PlataformName);
			_capabilities.SetCapability("platformVersion", ProjectConfig.PlatformVersion);
			_capabilities.SetCapability("deviceName", ProjectConfig.DeviceName);
			_capabilities.SetCapability("appPackage", ProjectConfig.AppPackage);
			_capabilities.SetCapability("newCommandTimeout", "3000");
			_capabilities.SetCapability("sessionOverride", "true");

			Uri defaultUri = new Uri(ProjectConfig.AppiumServer);

			if (ProjectConfig.PlataformName == "Android")
			{
			    _capabilities.SetCapability("app", ProjectConfig.AppPath);
				_capabilities.SetCapability("appActivity", ProjectConfig.AppActivity);

				_driver = new AndroidDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
			}
			else if (ProjectConfig.PlataformName == "iOS")
			{
                //_capabilities.SetCapability("automationName", "XCUITest");
                _capabilities.SetCapability("app", ProjectConfig.AppPath);
			    _capabilities.SetCapability("bundleId", ProjectConfig.AppPackage);
 				_capabilities.SetCapability("udid", ProjectConfig.Uuid);
				_driver = new IOSDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
			}

            System.Threading.Thread.Sleep(5000);


            Exec.Create("TodoListCordova", ProjectConfig.OutputDeviceID, "F2", 2,"CombinedExpressionsMultiLocator", ProjectConfig.OutputPath);
            Exec.Instance.Start();

            _locator = new LocatorStrategy(_driver, Exec.Instance);



            /*initialing test*/
            inserirTextoSendKeys_Test(); //inserirTexto
            btnAdicionarSendClick_Test(); //btnAdicionar			
            alterarTextoSendKeys_Test(); //alterarTexto
            tocarForaSendClick_Test(); //tocarFora

            Exec.Instance.EndSuccefull();


        }

        public void inserirTextoSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("inserirTexto");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.widget.EditText", @"//*[@resource-id='new-todo']", @"//*[@text='What needs to be done?' or @value='What needs to be done?']", @"//*/android.widget.EditText[@text='What needs to be done?']", @"//*[@resource-id='header']/*[2]", @"//*[@resource-id='header']//*[@text='What needs to be done?']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeTextField", @"", @"//*[@text='What needs to be done?' or @value='What needs to be done?']", @"//*/XCUIElementTypeTextField[@value='What needs to be done?']", @"//*[@label='region']/*[1]/*[1]", @"//*[@label='region']//*[@value='What needs to be done?']" };
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeTextField";
            }

            string[] selectorsType = new string[] { @"AbsolutePath", @"IdentifyAttributes", @"CrossPlatform", @"ElementType", @"AncestorIndex", @"AncestorAttributes" };

            IWebElement e = _locator.FindElementByXPathMultiLocator(selectors, selectorsType);

            if (e == null)
            {
                e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);
                if (e != null)
                    Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;
            }

            e.Click();
            e.Clear();
            e.SendKeys("Tarefa1");
            try
            {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            }
            catch { }

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void btnAdicionarSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnAdicionar");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.widget.Button", @"//*[@resource-id='add-todo']", @"//*[@content-desc='+' or @label='+']", @"//*/android.widget.Button[@content-desc='+']", @"//*[@resource-id='header']/*[3]", @"//*[@resource-id='header']//*[@content-desc='+']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.widget.Button";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeButton", @"//*[@name='+']", @"//*[@content-desc='+' or @label='+']", @"//*/XCUIElementTypeButton[@label='+']", @"//*[@label='region']/*[1]/*[2]", @"//*[@label='region']//*[@label='+']" };
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeButton";
            }

            string[] selectorsType = new string[] { @"AbsolutePath", @"IdentifyAttributes", @"CrossPlatform", @"ElementType", @"AncestorIndex", @"AncestorAttributes" };

            IWebElement e = _locator.FindElementByXPathMultiLocator(selectors, selectorsType);

            if (e == null)
            {
                e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);
                if (e != null)
                    Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;
            }

            e.Click();

            /*Insert your assert here*/
            System.Threading.Thread.Sleep(2000);

            if (ProjectConfig.IndexDeviceUnderTest == 0)
            {
                _driver.FindElementById("com.android.packageinstaller:id/permission_allow_button").Click();
            }

            Exec.Instance.CurrentEvent.EndSucessfull();

        }

        public void alterarTextoSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("alterarTexto");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.widget.EditText", @"", @"//*[@text='Tarefa1' or @value='Tarefa1']", @"//*/android.widget.EditText[@text='Tarefa1']", @"//*[@resource-id='todo-list']/*[1]/*[1]", @"//*[@resource-id='todo-list']//*[@text='Tarefa1']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeTextField", @"", @"//*[@text='Tarefa1' or @value='Tarefa1']", @"//*/XCUIElementTypeTextField[@value='Tarefa1']", @"//*[@label='region']/*[1]", @"//*[@label='region']//*[@value='Tarefa1']"};
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeTextField";
            }

            string[] selectorsType = new string[] {@"AbsolutePath", @"IdentifyAttributes", @"CrossPlatform", @"ElementType", @"AncestorIndex", @"AncestorAttributes"};

            IWebElement e = _locator.FindElementByXPathMultiLocator(selectors, selectorsType);

            if (e == null)
            {
                e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);
                if (e != null)
                    Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;
            }

            e.Click();
            e.Clear();
            e.SendKeys("Tarefa1a");
            try {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            } catch {}

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void tocarForaSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("tocarFora");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View", @"//*[@resource-id='todoapp']", @"//*[@label='CordovaToDoApp_AngularJS']", @"//*/android.view.View[@resource-id='todoapp']", @"//*[@content-desc='CordovaToDoApp_AngularJS']/*[1]", @"//*[@content-desc='CordovaToDoApp_AngularJS']//*[@resource-id='todoapp']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther", @"//*[@name='CordovaToDoApp_AngularJS']", @"//*[@label='CordovaToDoApp_AngularJS']", @"//*/XCUIElementTypeOther[@label='CordovaToDoApp_AngularJS']", @"//*[@label='AngularJSTodoApp']/*[1]/*[1]/*[1]/*[1]/*[1]/*[1]", @"//*[@label='AngularJSTodoApp']//*[@label='CordovaToDoApp_AngularJS']"};
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther";
            }

            string[] selectorsType = new string[] {@"AbsolutePath", @"IdentifyAttributes", @"CrossPlatform", @"ElementType", @"AncestorIndex", @"AncestorAttributes"};

            IWebElement e = _locator.FindElementByXPathMultiLocator(selectors, selectorsType);

            if (e == null)
            {
                e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);
                if (e != null)
                    Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;
            }

            e.Click();

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }





		private void ForceUpdateScreen()
		{
            //workaround for forces screen update 
            System.Threading.Thread.Sleep(500);
            string x = _driver.PageSource;
            System.Threading.Thread.Sleep(500);

        }


        private void ScrollToBottom()
        {
            var size = _driver.Manage().Window.Size;
            int startx = size.Width / 2;
            int starty = (int)(size.Height * 0.9);
            int endx = size.Width / 2;
            int endy = (int)(size.Height * 0.2);
            _driver.Swipe(startx, starty, startx, endy, 100);

            System.Threading.Thread.Sleep(1000);


        }

        private void Tap(int x, int y)
        {
            _driver.Tap(1, x, y, 500);

        }
    }
}
