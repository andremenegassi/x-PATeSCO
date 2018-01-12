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
    public class F3CombinedExpressionsMultiLocator
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
				_driver = new AndroidDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
			}
			else if (ProjectConfig.PlataformName == "iOS")
			{
                _capabilities.SetCapability("automationName", "XCUITest");
                _capabilities.SetCapability("app", ProjectConfig.AppPath);
			    _capabilities.SetCapability("bundleId", ProjectConfig.AppPackage);
 				_capabilities.SetCapability("udid", ProjectConfig.Uuid);
				_driver = new IOSDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
			}



            Exec.Create("PedidoApp", ProjectConfig.OutputDeviceID, "F3", 4,"CombinedExpressionsMultiLocator", ProjectConfig.OutputPath);
            Exec.Instance.Start();

            _locator = new LocatorStrategy(_driver, Exec.Instance);



            /*initialing test*/
            System.Threading.Thread.Sleep(3000);

            selboloSendClick_Test(); //selbolo
            btnpedirSendClick_Test(); //btnpedir
            btnconfirmarSendClick_Test(); //btnconfirmar
            btnokSendClick_Test(); //btnok

            Exec.Instance.EndSuccefull();


        }

        public void selboloSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("selbolo");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[3]/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View", @"", @"//*[@content-desc='De Brigadeiro apenas R$ 24' or @label='De Brigadeiro']", @"//*/android.view.View[@content-desc='De Brigadeiro apenas R$ 24']", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[3]/*[1]/*[1]/*[1]/*[1]/*[1]/*[1]", @"//*[@resource-id='android:id/content']//*[@content-desc='De Brigadeiro apenas R$ 24']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[3]/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View/android.view.View";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIALink[3]/UIALink", @"//*[@name='De Brigadeiro']", @"//*[@content-desc='De Brigadeiro apenas R$ 24' or @label='De Brigadeiro']", @"//*/UIALink[@label='De Brigadeiro']", @"//*[@label='De Brigadeiro apenas R$ 24']/*[2]", @"//*[@label='De Brigadeiro apenas R$ 24']//*[@label='De Brigadeiro']"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIALink[3]/UIALink";
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


        public void btnpedirSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnpedir");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[3]/android.view.View/android.view.View/android.view.View/android.view.View[2]", @"", @"//*[@content-desc='Pedir esse bolo agora ' or @label='Pedir esse bolo agora']", @"//*/android.view.View[@content-desc='Pedir esse bolo agora ']", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[3]/*[1]/*[1]/*[1]/*[3]", @"//*[@resource-id='android:id/content']//*[@content-desc='Pedir esse bolo agora ']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[3]/android.view.View/android.view.View/android.view.View/android.view.View[2]";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIALink", @"//*[@name='Pedir esse bolo agora']", @"//*[@content-desc='Pedir esse bolo agora ' or @label='Pedir esse bolo agora']", @"//*/UIALink[@label='Pedir esse bolo agora']", @"//*[@label='pedidoapp']/*[1]/*[2]/*[1]/*[5]", @"//*[@label='pedidoapp']//*[@label='Pedir esse bolo agora']"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIALink";
            }

            string[] selectorsType = new string[] {@"AbsolutePath", @"IdentifyAttributes", @"CrossPlatform", @"ElementType", @"AncestorIndex", @"AncestorAttributes"};

            IWebElement e = _locator.FindElementByXPathMultiLocator(selectors, selectorsType);

            if (e == null)
            {
                e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);
                if (e != null)
                    Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;
            }

            if (new int[] { 2 }.Contains(ProjectConfig.IndexDeviceUnderTest))
            {
                try
                {
                    Tap(360, 524);
                }
                catch { }
            }
            else if (new int[] { 4 }.Contains(ProjectConfig.IndexDeviceUnderTest))
                throw new Exception();
            else e.Click();

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void btnconfirmarSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnconfirmar");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[3]/android.view.View/android.view.View/android.view.View/android.view.View[2]/android.widget.Button", @"", @"//*[@content-desc='Confirmar Pedido! ' or @label='Confirmar Pedido!']", @"//*/android.widget.Button[@content-desc='Confirmar Pedido! ']", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[3]/*[1]/*[1]/*[1]/*[2]/*[5]", @"//*[@resource-id='android:id/content']//*[@content-desc='Confirmar Pedido! ']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[3]/android.view.View/android.view.View/android.view.View/android.view.View[2]/android.widget.Button";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIAButton[2]", @"//*[@name='Confirmar Pedido!']", @"//*[@content-desc='Confirmar Pedido! ' or @label='Confirmar Pedido!']", @"//*/UIAButton[@label='Confirmar Pedido!']", @"//*[@label='pedidoapp']/*[1]/*[2]/*[1]/*[9]", @"//*[@label='pedidoapp']//*[@label='Confirmar Pedido!']"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIAButton[2]";
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


        public void btnokSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnok");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[5]/android.view.View/android.widget.Button", @"", @"//*[@content-desc='OK ' or @label='OK']", @"//*/android.widget.Button[@content-desc='OK ']", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[5]/*[1]/*[3]", @"//*[@resource-id='android:id/content']//*[@content-desc='OK ']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View[5]/android.view.View/android.widget.Button";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIAButton[3]", @"//*[@name='OK']", @"//*[@content-desc='OK ' or @label='OK']", @"//*/UIAButton[@label='OK']", @"//*[@label='pedidoapp']/*[1]/*[2]/*[1]/*[12]", @"//*[@label='pedidoapp']//*[@label='OK']"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView[2]/UIAWebView/UIAButton[3]";
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
