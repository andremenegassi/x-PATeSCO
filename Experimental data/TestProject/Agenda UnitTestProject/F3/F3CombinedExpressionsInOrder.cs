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

namespace UnitTestProject.F3
{

    [TestClass]
    public class F3CombinedExpressionsInOrder
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

            if (ProjectConfig.IndexDeviceUnderTest == 0)
                _driver.FindElementById("com.android.packageinstaller:id/permission_allow_button").Click();


            Exec.Create("Agenda", ProjectConfig.OutputDeviceID, "F3", 4,"CombinedExpressionsInOrder", ProjectConfig.OutputPath);
            Exec.Instance.Start();

            _locator = new LocatorStrategy(_driver, Exec.Instance);


			 
			 /*initialing test*/
			
            btnMenuSendClick_Test(); //btnMenu
            btnPesquisaSendClick_Test(); //btnPesquisa
            btnInserirPesquisaSendKeys_Test(); //btnInserirPesquisa
            btnPesquisaPessoaSendClick_Test(); //btnPesquisaPessoa

            Exec.Instance.EndSuccefull();


        }

        public void btnMenuSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnMenu");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"//*[@content-desc='' or @label='']", @"//*/android.widget.Button[@content-desc='']", @"", @"//*[@content-desc='agenda']//*[@content-desc='']", @"//*[@content-desc='agenda']/*[1]/*[1]/*[1]/*[1]", @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View/android.widget.Button"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View/android.widget.Button";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"//*[@content-desc='' or @label='']", @"//*/XCUIElementTypeButton[@label='']", @"//*[@name='']", @"//*[@label='banner']//*[@label='']", @"//*[@label='banner']/*[1]", @"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeButton"};
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeButton";
            }

            string[] selectorsType = new string[] {@"CrossPlatform", @"ElementType", @"IdentifyAttributes", @"AncestorAttributes", @"AncestorIndex", @"AbsolutePath"};

            IWebElement e = _locator.FindElementByXPathInOrder(selectors, selectorsType);

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


        public void btnPesquisaSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnPesquisa");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"//*[@content-desc='Pesquisa' or @label='Pesquisa']", @"//*/android.view.View[@content-desc='Pesquisa']", @"", @"//*[@content-desc='agenda']//*[@content-desc='Pesquisa']", @"//*[@content-desc='agenda']/*[1]/*[1]/*[2]/*[2]/*[3]", @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View[2]/android.view.View[3]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View[2]/android.view.View[3]";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"//*[@content-desc='Pesquisa' or @label='Pesquisa']", @"//*/XCUIElementTypeLink[@label='Pesquisa']", @"//*[@name='Pesquisa']", @"//*[@label='navigation']//*[@label='Pesquisa']", @"//*[@label='navigation']/*[3]", @"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[3]/XCUIElementTypeLink[3]"};
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[3]/XCUIElementTypeLink[3]";
            }

            string[] selectorsType = new string[] {@"CrossPlatform", @"ElementType", @"IdentifyAttributes", @"AncestorAttributes", @"AncestorIndex", @"AbsolutePath"};

            IWebElement e = _locator.FindElementByXPathInOrder(selectors, selectorsType);

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


        public void btnInserirPesquisaSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnInserirPesquisa");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"//*[@text='Pesquisa ']", @"//*/android.widget.EditText[@text='Pesquisa ']", @"//*[@resource-id='txtPesquisa']", @"//*[@resource-id='pgPesquisa']//*[@text='Pesquisa ']", @"//*[@resource-id='pgPesquisa']/*[1]/*[1]", @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View/android.widget.EditText"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"//*[@text='Pesquisa ']", @"", @"", @"//*[@label='main']//*[]", @"//*[@label='main']/*[1]/*[1]", @"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeOther/XCUIElementTypeTextField"};
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeOther/XCUIElementTypeTextField";
            }

            string[] selectorsType = new string[] {@"CrossPlatform", @"ElementType", @"IdentifyAttributes", @"AncestorAttributes", @"AncestorIndex", @"AbsolutePath"};

            IWebElement e = _locator.FindElementByXPathInOrder(selectors, selectorsType);

            if (e == null)
            {
                e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);
                if (e != null)
                    Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;
            }

            e.Click();
            e.Clear();
            e.SendKeys("Andre");
            try {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            } catch {}

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void btnPesquisaPessoaSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnPesquisaPessoa");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"//*[@label='search']", @"//*/android.view.View[@resource-id='btnPesquisar']", @"//*[@resource-id='btnPesquisar']", @"//*[@resource-id='pgPesquisa']//*[@resource-id='btnPesquisar']", @"//*[@resource-id='pgPesquisa']/*[2]", @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[2]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[2]";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"//*[@label='search']", @"//*/XCUIElementTypeStaticText[@label='search']", @"//*[@name='search']", @"//*[@label='main']//*[@label='search']", @"//*[@label='main']/*[1]/*[3]/*[1]/*[1]", @"XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeOther/XCUIElementTypeStaticText"};
                contingencyXPathSelector = "XCUIElementTypeWindow/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeOther/XCUIElementTypeOther[2]/XCUIElementTypeOther/XCUIElementTypeStaticText";
            }

            string[] selectorsType = new string[] {@"CrossPlatform", @"ElementType", @"IdentifyAttributes", @"AncestorAttributes", @"AncestorIndex", @"AbsolutePath"};

            IWebElement e = _locator.FindElementByXPathInOrder(selectors, selectorsType);

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
