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

namespace UnitTestProject.F1
{

    [TestClass]
    public class F1CombinedExpressionsMultiLocator
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
                _capabilities.SetCapability("app", ProjectConfig.AppPath);
                _capabilities.SetCapability("bundleId", ProjectConfig.AppPackage);
                _capabilities.SetCapability("udid", ProjectConfig.Uuid);
                _driver = new IOSDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
            }



            Exec.Create("FFF", ProjectConfig.OutputDeviceID, "F1", 5, "CombinedExpressionsMultiLocator", ProjectConfig.OutputPath);
            Exec.Instance.Start();

            _locator = new LocatorStrategy(_driver, Exec.Instance);



            /*initialing test*/

            btnsearchSendClick_Test(); //btnsearch
            tpproductSendClick_Test(); //tpproduct
            tppaymentSendClick_Test(); //tppayment
            btnsearch2SendClick_Test(); //btnsearch2
            mktSendClick_Test(); //mkt

            Exec.Instance.EndSuccefull();


        }

        public void btnsearchSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnsearch");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[3]", @"//*[@resource-id='search']", @"//*[@content-desc='Search For a Market' or @label='Search For a Market']", @"//*/android.view.View[@content-desc='Search For a Market']", @"//*[@resource-id='defaultView']/*[3]", @"//*[@resource-id='defaultView']//*[@content-desc='Search For a Market']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[3]";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIALink[2]/UIAStaticText", @"//*[@name='Search For a Market']", @"//*[@content-desc='Search For a Market' or @label='Search For a Market']", @"//*/UIAStaticText[@label='Search For a Market']", @"//*[@label='Search For a Market']/*[1]", @"//*[@label='Search For a Market']//*[@label='Search For a Market']" };
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIALink[2]/UIAStaticText";
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

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void tpproductSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("tpproduct");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[9]/android.widget.CheckBox", @"//*[@resource-id='search_bakedGoods']", @"//*[@content-desc='Baked Goods' or @label='Baked Goods']", @"//*/android.widget.CheckBox[@content-desc='Baked Goods']", @"//*[@resource-id='searchView']/*[10]/*[1]", @"//*[@resource-id='searchView']//*[@content-desc='Baked Goods']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[9]/android.widget.CheckBox";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIAStaticText[7]", @"//*[@name='Baked Goods']", @"//*[@content-desc='Baked Goods' or @label='Baked Goods']", @"//*/UIAStaticText[@label='Baked Goods']", @"//*[@label='Fresh-Food-Finder']/*[1]/*[1]/*[1]/*[11]", @"//*[@label='Fresh-Food-Finder']//*[@label='Baked Goods']" };
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIAStaticText[7]";
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

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void tppaymentSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("tppayment");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[29]/android.widget.CheckBox", @"//*[@resource-id='search_credit']", @"//*[@content-desc='Credit Cards Accepted' or @label='Credit Cards Accepted']", @"//*/android.widget.CheckBox[@content-desc='Credit Cards Accepted']", @"//*[@resource-id='searchView']/*[30]/*[1]", @"//*[@resource-id='searchView']//*[@content-desc='Credit Cards Accepted']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[29]/android.widget.CheckBox";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIAStaticText[23]", @"//*[@name='Credit Cards Accepted']", @"//*[@content-desc='Credit Cards Accepted' or @label='Credit Cards Accepted']", @"//*/UIAStaticText[@label='Credit Cards Accepted']", @"//*[@label='Fresh-Food-Finder']/*[1]/*[1]/*[1]/*[42]", @"//*[@label='Fresh-Food-Finder']//*[@label='Credit Cards Accepted']" };
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIAStaticText[23]";
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

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void btnsearch2SendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnsearch2");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[36]", @"//*[@resource-id='searchButton']", @"//*[@content-desc='Search' or @label='Search']", @"//*/android.view.View[@content-desc='Search']", @"//*[@resource-id='searchView']/*[37]", @"//*[@resource-id='searchView']//*[@content-desc='Search']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.view.View[36]";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIALink[2]", @"//*[@name='Search']", @"//*[@content-desc='Search' or @label='Search']", @"//*/UIALink[@label='Search']", @"//*[@label='Fresh-Food-Finder']/*[1]/*[1]/*[1]/*[51]", @"//*[@label='Fresh-Food-Finder']//*[@label='Search']" };
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIALink[2]";
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

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void mktSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("mkt");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] { @"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.widget.ListView/android.view.View", @"", @"//*[@content-desc='Calera Farmers Market
9758 Hwy 25, Calera, Al 35040
Calera, Alabama, 35040' or @label='Calera Farmers Market']", @"//*/android.view.View[@content-desc='Calera Farmers Market
9758 Hwy 25, Calera, Al 35040
Calera, Alabama, 35040']", @"//*[@resource-id='searchResultsView']/*[2]/*[1]", @"//*[@resource-id='searchResultsView']//*[@content-desc='Calera Farmers Market
9758 Hwy 25, Calera, Al 35040
Calera, Alabama, 35040']" };
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.webkit.WebView/android.webkit.WebView/android.view.View/android.view.View/android.view.View[2]/android.view.View/android.view.View/android.widget.ListView/android.view.View";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] { @"AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIAStaticText[6]", @"//*[@name='Calera Farmers Market']", @"//*[@content-desc='Calera Farmers Market
9758 Hwy 25, Calera, Al 35040
Calera, Alabama, 35040' or @label='Calera Farmers Market']", @"//*/UIAStaticText[@label='Calera Farmers Market']", @"//*[@label='Fresh-Food-Finder']/*[1]/*[1]/*[1]/*[6]", @"//*[@label='Fresh-Food-Finder']//*[@label='Calera Farmers Market']" };
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIAScrollView/UIAWebView/UIAStaticText[6]";
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
