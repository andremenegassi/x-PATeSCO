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
				_capabilities.SetCapability("appActivity", ProjectConfig.AppActivity);

				_driver = new AndroidDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
			}
			else if (ProjectConfig.PlataformName == "iOS")
			{
			    _capabilities.SetCapability("app", ProjectConfig.AppPath);
			    _capabilities.SetCapability("bundleId", ProjectConfig.AppPackage);
 				_capabilities.SetCapability("udid", ProjectConfig.Uuid);
				_driver = new IOSDriver<IWebElement>(defaultUri, _capabilities, TimeSpan.FromSeconds(3000));
			}


            System.Threading.Thread.Sleep(15000);

            Exec.Create("Ofertados", ProjectConfig.OutputDeviceID, "F1", 6,"CombinedExpressionsMultiLocator", ProjectConfig.OutputPath);
            Exec.Instance.Start();

            _locator = new LocatorStrategy(_driver, Exec.Instance);


			 
			 /*initialing test*/
			
            btnaddSendClick_Test(); //btnadd
            inserirnomeSendKeys_Test(); //inserirnome
            inserirdescricaoSendKeys_Test(); //inserirdescricao
            inserirprecoantigoSendKeys_Test(); //inserirprecoantigo
            inserirpreconovoSendKeys_Test(); //inserirpreconovo
            btnsalvarSendClick_Test(); //btnsalvar

            Exec.Instance.EndSuccefull();


        }

        public void btnaddSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnadd");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.widget.FrameLayout/android.widget.ImageButton", @"", @"//*[@label='add']", @"", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[2]/*[1]/*[1]/*[2]/*[1]/*[1]", @"//*[@resource-id='android:id/content']//*[]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.widget.FrameLayout/android.widget.ImageButton";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIANavigationBar/UIAButton[2]", @"//*[@name='add']", @"//*[@label='add']", @"//*/UIAButton[@label='add']", @"//*[@name='Ofertados']/*[4]", @"//*[@name='Ofertados']//*[@label='add']"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIANavigationBar/UIAButton[2]";
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


        public void inserirnomeSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("inserirnome");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[2]/android.widget.LinearLayout/android.widget.EditText", @"", @"", @"", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[2]/*[1]/*[1]/*[1]/*[1]/*[2]/*[1]/*[2]", @"//*[@resource-id='android:id/content']//*[]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[2]/android.widget.LinearLayout/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell/UIATextField", @"", @"", @"", @"//*[@name='Nome']/*[2]", @"//*[@name='Nome']//*[]"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell/UIATextField";
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
            e.SendKeys("Teste");
            try {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            } catch {}

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void inserirdescricaoSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("inserirdescricao");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[3]/android.widget.LinearLayout/android.widget.EditText", @"", @"", @"", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[2]/*[1]/*[1]/*[1]/*[1]/*[3]/*[1]/*[2]", @"//*[@resource-id='android:id/content']//*[]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[3]/android.widget.LinearLayout/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell[2]/UIATextField", @"", @"", @"", @"//*[@name='Descrição']/*[2]", @"//*[@name='Descrição']//*[]"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell[2]/UIATextField";
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
            e.SendKeys("Teste 2");
            try {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            } catch {}

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void inserirprecoantigoSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("inserirprecoantigo");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[5]/android.widget.LinearLayout/android.widget.EditText", @"", @"", @"", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[2]/*[1]/*[1]/*[1]/*[1]/*[5]/*[1]/*[2]", @"//*[@resource-id='android:id/content']//*[]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[5]/android.widget.LinearLayout/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell[3]/UIATextField", @"", @"", @"", @"//*[@name='Preço Antigo']/*[2]", @"//*[@name='Preço Antigo']//*[]"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell[3]/UIATextField";
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
            e.SendKeys("1");
            try {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            } catch {}

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void inserirpreconovoSendKeys_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("inserirpreconovo");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[6]/android.widget.LinearLayout/android.widget.EditText", @"", @"", @"", @"//*[@resource-id='android:id/content']/*[1]/*[1]/*[1]/*[2]/*[1]/*[1]/*[1]/*[1]/*[6]/*[1]/*[2]", @"//*[@resource-id='android:id/content']//*[]"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout[6]/android.widget.LinearLayout/android.widget.EditText";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell[4]/UIATextField", @"", @"", @"", @"//*[@name='Preço Novo']/*[2]", @"//*[@name='Preço Novo']//*[]"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIATableView/UIATableCell[4]/UIATextField";
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
            e.SendKeys("2");
            try {
                if (ProjectConfig.PlataformName == "Android")
                    _driver.HideKeyboard();
                else if (ProjectConfig.PlataformName == "iOS")
                    _driver.FindElementByXPath("//*[@name='Hide keyboard']").Click();
            } catch {}

            /*Insert your assert here*/

            Exec.Instance.CurrentEvent.EndSucessfull();

        }


        public void btnsalvarSendClick_Test()
        {
            ForceUpdateScreen();
            Exec.Instance.AddEvent("btnsalvar");

            string[] selectors = new string[0];
            string contingencyXPathSelector = "";

            if (ProjectConfig.PlataformName == "Android")
            {
                selectors = new string[] {@"hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.support.v7.widget.LinearLayoutCompat/android.widget.TextView", @"", @"//*[@content-desc='Save' or @label='save']", @"//*/android.widget.TextView[@content-desc='Save']", @"//*[@resource-id='com.xamarin.acquaintforms:id/toolbar']/*[2]/*[1]", @"//*[@resource-id='com.xamarin.acquaintforms:id/toolbar']//*[@content-desc='Save']"};
                contingencyXPathSelector = "hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup/android.support.v7.widget.LinearLayoutCompat/android.widget.TextView";
            }
            else if (ProjectConfig.PlataformName == "iOS")
            {
                selectors = new string[] {@"AppiumAUT/UIAApplication/UIAWindow/UIANavigationBar/UIAButton[3]", @"//*[@name='save']", @"//*[@content-desc='Save' or @label='save']", @"//*/UIAButton[@label='save']", @"//*[@name='Xamarin_Forms_Platform_iOS_NavigationRenderer_ParentingView']/*[4]", @"//*[@name='Xamarin_Forms_Platform_iOS_NavigationRenderer_ParentingView']//*[@label='save']"};
                contingencyXPathSelector = "AppiumAUT/UIAApplication/UIAWindow/UIANavigationBar/UIAButton[3]";
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
