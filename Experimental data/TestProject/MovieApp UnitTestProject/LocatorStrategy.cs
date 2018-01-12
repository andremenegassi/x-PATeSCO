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
            List<MultiLocatorElement> elementsAux = new List<MultiLocatorElement>();
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

                    if (selector != "")
                    {
                        e = _driver.FindElementByXPath(selector);

                        if (_execEvaluation != null)
                            _execEvaluation.CurrentEvent.CurrentSelector.EndSucessfull();
                    }
                }
                catch { }

                if (e != null)
                {
                    int index = -1;


                    MultiLocatorElement me = new MultiLocatorElement();
                    try
                    {
                        me.Displayed = e.Displayed;
                    }
                    catch { }

                    try
                    {
                        me.Location = e.Location;
                    }
                    catch { }

                    try
                    {
                        me.Size = e.Size;
                    }
                    catch { }

                    try
                    {
                        me.Enabled = e.Enabled;
                    }
                    catch { }

                    try
                    {
                        me.Selected = e.Selected;
                    }
                    catch { }

                    try
                    {
                        me.Text = e.Text;
                    }
                    catch { }

                    try
                    {
                        me.TagName = e.TagName;
                    }
                    catch { }
                    

            
                    index = elementsAux.FindIndex(o => o.Displayed == me.Displayed &&
                                                    o.Location == me.Location &&
                                                    o.Size == me.Size &&
                                                    o.Enabled == me.Enabled &&
                                                    o.Selected == me.Selected &&
                                                    o.Text == me.Text &&
                                                    o.TagName == me.TagName);
                    
                   
                    if (index == -1)
                    {
                        elements.Add(e);
                        elementsAux.Add(me);
                        voting.Add(0);
                        index = elements.Count - 1;
                    }

                    voting[index] += ExtractWeight(selectorType);
                }
            }

            if (elementsAux.Count == 0)
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

    internal class MultiLocatorElement
    { 
        public bool Displayed { get; set; }
        public System.Drawing.Point Location { get; set; }
        public System.Drawing.Size Size { get; set; }
        public bool Enabled { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string TagName { get; set; }

        public MultiLocatorElement()
        {
            Displayed = false;
            Location = System.Drawing.Point.Empty;
            Size = System.Drawing.Size.Empty;
            Enabled = false;
            Selected = false;
            Text = "";
            TagName = "";

        }


    }



}
