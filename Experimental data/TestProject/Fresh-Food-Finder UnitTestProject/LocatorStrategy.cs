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

namespace UnitTestProject
{
    public class LocatorStrategy 
    {

        AppiumDriver<IWebElement> _driver = null;
		Exec _execEvaluation = null;

        public LocatorStrategy(AppiumDriver<IWebElement> driver, Exec execEvaluation)
        {
            _driver = driver;
			_execEvaluation = execEvaluation;
        }

        /// <summary>
        /// Strategy Individual Expression
        /// </summary>
        /// <param name="selectors"></param>
        /// <param name="selectorsType"></param>
        /// <returns></returns>
        public IWebElement FindElementByXPath(string selector, string selectorType)
        {
            IWebElement e = null;
            selector = selector.Replace("\r", string.Empty);

            try
            {
				if (_execEvaluation != null)
					_execEvaluation.CurrentEvent.AddSelector(selectorType, selector);

                e = _driver.FindElementByXPath(selector);

				if (_execEvaluation != null)
					_execEvaluation.CurrentEvent.CurrentSelector.EndSucessfull();

            }
            catch { }

            return e;
        }

		/// <summary>
        /// Strategy Individual Expression
        /// </summary>
        /// <param name="selectors"></param>
        /// <returns></returns>
        public IWebElement FindElementByContingencyXPath(string selector)
        {
            IWebElement e = null;

            try
            {
                e = _driver.FindElementByXPath(selector);
            }
            catch { }

            return e;
        }



        /// <summary>
        /// Combined Expression - In Order
        /// </summary>
        /// <param name="selectors"></param>
        /// <param name="selectorsType"></param>
        /// <returns></returns>
        public IWebElement FindElementByXPathInOrder(string[] selectors, string[] selectorsType)
        {
            IWebElement e = null;

            for (int i = 0; i < selectors.Length; i++)
            {
                string selector = selectors[i].Replace("\r", string.Empty);
                string selectorType = selectorsType[i];

                try
                {
					if (_execEvaluation != null)
						_execEvaluation.CurrentEvent.AddSelector(selectorType, selector);

					e = _driver.FindElementByXPath(selector);

					if (_execEvaluation != null)
						_execEvaluation.CurrentEvent.CurrentSelector.EndSucessfull();
                }
                catch { }
                if (e != null)
                    break;
            }

            return e;
        }


        /// <summary>
        /// Combined Expression - MultiLocator
        /// </summary>
        /// <param name="selectors"></param>
        /// <param name="selectorsType"></param>
        /// <returns></returns>
        public IWebElement FindElementByXPathMultiLocator(string[] selectors, string[] selectorsType)
        {
            List<IWebElement> elements = new List<IWebElement>();
            List<double> voting = new List<double>();

            for (int i = 0; i < selectors.Length; i++)
            {
                string selector = selectors[i].Replace("\r", string.Empty);
                string selectorType = selectorsType[i];

                IWebElement e = null;

                try
                {
					if (_execEvaluation != null)
						_execEvaluation.CurrentEvent.AddSelector(selectorType, selector);

					e = _driver.FindElementByXPath(selector);

					if (_execEvaluation != null)
						_execEvaluation.CurrentEvent.CurrentSelector.EndSucessfull();
                }
                catch { }

                if (e != null)
                {
                    int index = -1;

                    try
                    {
                        index = elements.FindIndex(o => o.Displayed == e.Displayed && o.Location == e.Location && o.Size == e.Size && o.Enabled == e.Enabled && o.Selected == e.Selected && o.Text == e.Text && o.TagName == e.TagName);
                    }
                    catch
                    {
                        index = elements.FindIndex(o => o.Displayed == e.Displayed && o.Location == e.Location && o.Size == e.Size && o.Enabled == e.Enabled && o.Text == e.Text && o.TagName == e.TagName);
                    }

                    if (index == -1)
                    {
                        elements.Add(e);
                        voting.Add(0);
                        index = elements.Count - 1;
                    }

                    voting[index] += ExtractWeight(selectorType);
                }
            }

            if (elements.Count == 0)
				return null;
            else
            {
                //Most voted Element
                return elements[voting.IndexOf(voting.Max())];
            }
        }

        private double ExtractWeight(string selectorType)
        {
            switch (selectorType.ToLower())
            {
                case "absolutepath": return 0.05;
                case "identifyattributes": return 0.15;
                case "crossplatform": return 0.25;
                case "elementtype": return 0.25;
                case "ancestorindex": return 0.05;
                case "ancestorattributes": return 0.25;
            }

            return 0;
        }
    }
}
