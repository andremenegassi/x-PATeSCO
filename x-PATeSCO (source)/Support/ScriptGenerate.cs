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
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;

namespace CrossPlatformCompatibility.Support
{

    public static class ScriptGenerate
    {
        public enum TpLocatorEstrategy
        {
            IndividualExpression = 1,
            CombinedExpressionsInOrder = 2,
            CombinedExpressionsMultiLocator = 3
        }

        static bool _enabledEvaluation = false;

        public static bool EnabledEvaluation
        {
            get { return _enabledEvaluation; }
            set { _enabledEvaluation = value; }
        }

        public static bool _onlyTestClass = false;

        public static bool OnlyTestClass
        {
            get { return _onlyTestClass; }
            set { _onlyTestClass = value; }
        }



        private static string _tab1 = "    ";
        private static string _tab2 = "        ";
        private static string _tab3 = "            ";
        private static string _tab4 = "                ";
        private static string _tab5 = "                    ";
        private static string _tab6 = "                        ";

        public static string GenerateScriptMethodTest(DeviceConfig deviceConfigAndroid, DeviceConfig deviceConfigIOS, TpLocatorEstrategy locatorEstrategy, XPathSelector.XPathType individualExpressionType, bool staticMethod, string appiumDriverName, out string functionCall, out string appiumConfig)
        {

            functionCall = "";
            appiumConfig = "";

            #region validation
            string msg = "";
     
            if (!Support.MyNode.CheckDeviceConfigCompatibility(deviceConfigAndroid, deviceConfigIOS, out msg))
            {
                return msg;
            }
            
            #endregion


            StringBuilder scriptCallFunction = new StringBuilder();
            List<string> functionList = new List<string>();
            List<string> scriptList = new List<string>();
            StringBuilder appiumConfigList = new StringBuilder();

            string elementName = "e";

            int qtdEvents = deviceConfigAndroid.Events.Count;
            for (int i = 0; i < qtdEvents; i++)
            {
                MyNode nAndroid = deviceConfigAndroid.Events[i];
                MyNode nIOS = deviceConfigIOS.Events[i];


                string scriptFunction = GenerateScriptMethodTest(nAndroid, nIOS, locatorEstrategy, individualExpressionType, staticMethod, elementName, appiumDriverName, out functionCall);

                if (!functionList.Contains(functionCall))
                {
                    functionList.Add(functionCall);
                    scriptList.Add(scriptFunction);
                    scriptCallFunction.AppendLine(_tab3 + functionCall);
                }
            }


            #region appium config
               

            appiumConfigList.AppendLine();

            if (!EnabledEvaluation)
            {
                appiumConfigList.AppendLine(_tab3 + "_locator = new LocatorStrategy(_driver, null);");
                appiumConfigList.AppendLine();
            }
            else
            {
                #region Evaluation

                string strategy = locatorEstrategy.ToString();
                if (locatorEstrategy == TpLocatorEstrategy.IndividualExpression)
                    strategy += individualExpressionType.ToString();

                appiumConfigList.AppendLine(_tab3 + "Exec.Create(\"" + deviceConfigAndroid.AppName + "\", ProjectConfig.OutputDeviceID, \"" + deviceConfigAndroid.TestCaseName + "\", " + deviceConfigAndroid.Events.Count + ",\"" + strategy + "\", ProjectConfig.OutputPath);");
                appiumConfigList.AppendLine(_tab3 + "Exec.Instance.Start();");
                #endregion
                appiumConfigList.AppendLine();
                appiumConfigList.AppendLine(_tab3 + "_locator = new LocatorStrategy(_driver, Exec.Instance);");
                appiumConfigList.AppendLine();

            }

            #endregion

            scriptCallFunction.AppendLine();
            scriptCallFunction.AppendLine(_tab3 + "Exec.Instance.EndSuccefull();");


            functionCall = scriptCallFunction.ToString();
            appiumConfig = appiumConfigList.ToString();

            return string.Join("\n", scriptList.ToArray());

        }

        public static string GenerateScriptMethodTest(MyNode nodeDeviceAndroid, MyNode nodeDeviceIOS, TpLocatorEstrategy locatorEstrategy, XPathSelector.XPathType individualExpressionType, bool staticMethod, string elementName, string appiumDriverName, out string functionCall)
        {

            List<XPathSelector> selectors = MyNode.ExtractSelectors(nodeDeviceAndroid, nodeDeviceIOS);

            #region Contingency Selector
            string contingencySelectorForAndroid = selectors.Where(s => s.Type == XPathSelector.XPathType.AbsolutePath).Single().XPathForAndroid;
            string contingencySelectorForIOS = selectors.Where(s => s.Type == XPathSelector.XPathType.AbsolutePath).Single().XPathForIOS;
            #endregion

            if (locatorEstrategy == TpLocatorEstrategy.IndividualExpression)
            {
                //Only a XPath
                selectors = selectors.Where(s => s.Type == individualExpressionType).ToList();
            }
            else if (locatorEstrategy == TpLocatorEstrategy.CombinedExpressionsInOrder)
            {
                selectors = MyNode.OrderBySelectorInOrder(selectors);
            }
            else if (locatorEstrategy == TpLocatorEstrategy.CombinedExpressionsMultiLocator)
            {
                //nothing
            }




 

            StringBuilder scriptFunction = new StringBuilder();
            StringBuilder scriptFunctionBody = new StringBuilder();

            string tpStatic = " ";

            if (staticMethod)
                tpStatic = " static ";

            string functionName = nodeDeviceAndroid.EventName.Replace(" ", string.Empty);

            if (nodeDeviceAndroid.SendClick)
            {
                functionName += "SendClick_Test";
            }
            else if (nodeDeviceAndroid.SendKeys)
            {
                functionName += "SendKeys_Test";
            }

            if (nodeDeviceAndroid.WaitElementBySecond > 0)
            {
                scriptFunctionBody.AppendLine(_tab3 + "System.Threading.Thread.Sleep(" + (nodeDeviceAndroid.WaitElementBySecond * 1000) + ");");
            }

           
            List<string> selectorsAndroid = new List<string>();
            List<string> selectorsIOS = new List<string>();
            List<string> selectorsType = new List<string>();

            foreach (var s in selectors)
            {
                selectorsAndroid.Add("@\"" + s.XPathForAndroid + "\"");
                selectorsIOS.Add("@\"" + s.XPathForIOS + "\"");
                selectorsType.Add("@\"" + s.Type.ToString() + "\"");
 
            }


            scriptFunctionBody.AppendLine(_tab3 + "ForceUpdateScreen();");

            #region
            if (EnabledEvaluation)
            {
                scriptFunctionBody.AppendLine(_tab3 + "Exec.Instance.AddEvent(\"" + nodeDeviceAndroid.EventName + "\");");
                scriptFunctionBody.AppendLine();
            }
            #endregion

            scriptFunctionBody.AppendLine(_tab3 + "string[] selectors = new string[0];");

            if (_enabledEvaluation)
                scriptFunctionBody.AppendLine(_tab3 + "string contingencyXPathSelector = \"\";");


            scriptFunctionBody.AppendLine();
            scriptFunctionBody.AppendLine(_tab3 + "if (ProjectConfig.PlataformName == \"Android\")");
            scriptFunctionBody.AppendLine(_tab3 + "{");
            scriptFunctionBody.AppendLine(_tab4 + "selectors = new string[] {" + string.Join(", ", selectorsAndroid) + "};");

            if (_enabledEvaluation)
                scriptFunctionBody.AppendLine(_tab4 + "contingencyXPathSelector = \"" +  contingencySelectorForAndroid + "\";");

            scriptFunctionBody.AppendLine(_tab3 + "}");
            scriptFunctionBody.AppendLine(_tab3 + "else if (ProjectConfig.PlataformName == \"iOS\")");
            scriptFunctionBody.AppendLine(_tab3 + "{");
            scriptFunctionBody.AppendLine(_tab4 + "selectors = new string[] {" + string.Join(", ", selectorsIOS) + "};");

            if (_enabledEvaluation)
                scriptFunctionBody.AppendLine(_tab4 + "contingencyXPathSelector = \"" + contingencySelectorForIOS + "\";");

            scriptFunctionBody.AppendLine(_tab3 + "}");

            scriptFunctionBody.AppendLine();
            scriptFunctionBody.AppendLine(_tab3 + "string[] selectorsType = new string[] {" + string.Join(", ", selectorsType) + "};");

            scriptFunctionBody.AppendLine();

            if (locatorEstrategy == TpLocatorEstrategy.IndividualExpression)
                scriptFunctionBody.AppendLine(_tab3 + "IWebElement " + elementName + " = _locator.FindElementByXPath(selectors[0], selectorsType[0]);");
            else if (locatorEstrategy == TpLocatorEstrategy.CombinedExpressionsInOrder)
                scriptFunctionBody.AppendLine(_tab3 + "IWebElement " + elementName + " = _locator.FindElementByXPathInOrder(selectors, selectorsType);");
            else if (locatorEstrategy == TpLocatorEstrategy.CombinedExpressionsMultiLocator)
                scriptFunctionBody.AppendLine(_tab3 + "IWebElement " + elementName + " = _locator.FindElementByXPathMultiLocator(selectors, selectorsType);");

            scriptFunctionBody.AppendLine();

            if (_enabledEvaluation)
            {
                scriptFunctionBody.AppendLine(_tab3 + "if (e == null)");
                scriptFunctionBody.AppendLine(_tab3 + "{");
                scriptFunctionBody.AppendLine(_tab4 + "e = _locator.FindElementByContingencyXPath(contingencyXPathSelector);");
                scriptFunctionBody.AppendLine(_tab4 + "if (e != null)");
                scriptFunctionBody.AppendLine(_tab5 + "Exec.Instance.CurrentEvent.UsedContingencyXPathSelector = true;");
                scriptFunctionBody.AppendLine(_tab3 + "}");
                scriptFunctionBody.AppendLine();

            }


            if (nodeDeviceAndroid.SendClick)
            {
                scriptFunctionBody.Append(_tab3 + elementName + ".Click();");
            }
            else if (nodeDeviceAndroid.SendKeys)
            {
                scriptFunctionBody.AppendLine(_tab3 + elementName + ".Click();");
                scriptFunctionBody.AppendLine(_tab3 + elementName + ".Clear();");
                scriptFunctionBody.AppendLine(_tab3 + elementName + ".SendKeys(\"" + nodeDeviceAndroid.SendKeysText + "\");");
                scriptFunctionBody.AppendLine(_tab3 + "try {");
                scriptFunctionBody.AppendLine(_tab4 + "if (ProjectConfig.PlataformName == \"Android\")");
                scriptFunctionBody.Append(_tab5 + appiumDriverName + ".HideKeyboard();");
                scriptFunctionBody.AppendLine();
                scriptFunctionBody.AppendLine(_tab4 + "else if (ProjectConfig.PlataformName == \"iOS\")");
                scriptFunctionBody.Append(_tab5 + appiumDriverName + ".FindElementByXPath(\"//*[@name='Hide keyboard']\").Click();");
                scriptFunctionBody.AppendLine();
                scriptFunctionBody.Append(_tab3 + "} catch {}");
            }
            scriptFunctionBody.AppendLine();
            scriptFunctionBody.AppendLine();
            scriptFunctionBody.AppendLine(_tab3 + "/*Insert your assert here*/");


            #region
            if (EnabledEvaluation)
            {

                scriptFunctionBody.AppendLine();
                scriptFunctionBody.AppendLine(_tab3 + "Exec.Instance.CurrentEvent.EndSucessfull();");
            }
            #endregion

            scriptFunction.AppendLine(_tab2 + "public" + tpStatic + "void " + functionName + "()");
            scriptFunction.AppendLine(_tab2 + "{");
            scriptFunction.AppendLine(scriptFunctionBody.ToString());
            scriptFunction.AppendLine(_tab2 + "}");
            scriptFunction.AppendLine("");

            functionCall = functionName + "(); //" + nodeDeviceAndroid.EventName;

            return scriptFunction.ToString();

        }


        public static void InterpretString(AppiumDriver<IWebElement> driver, string otherFunction)
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            string exeDir = Path.GetDirectoryName(exePath);

            AssemblyName[] assemRefs = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            List<string> references = new List<string>();

            foreach (AssemblyName assemblyName in assemRefs)
                references.Add(assemblyName.Name + ".dll");

            for (int i = 0; i < references.Count; i++)
            {
                string localName = Path.Combine(exeDir, references[i]);

                if (File.Exists(localName))
                    references[i] = localName;
            }


            string compilation_string =
            @"  using Newtonsoft.Json;
                using OpenQA.Selenium;
                using OpenQA.Selenium.Remote;
                using System;
                using System.Collections.Generic;
                using System.Drawing;
                using System.IO;
                using System.Linq;
                using System.Xml;
                using System.Net;
                using System.Windows.Forms;
                using System.Xml.Linq;
                using CrossPlatformCompatibility.AppiumPageObject;
                using Microsoft.CSharp;
                using System.CodeDom.Compiler;
                using System.Text;
                using System.Reflection;
                using Microsoft.VisualStudio.TestTools.UnitTesting;

                static class RuntimeCompilationScriptTestCode { 
                    public static AppiumDriverPageObject<IWebElement> Driver { get; set; }

                    public static void Main() {}  
                    public static void ExecuteScript() {

                        /* SCRIPT CODE HERE  */
                      
                    }

                    /* OTHER FUNCTIONS */

                }";
            //Replace("/* SCRIPT CODE HERE */", script).
            compilation_string = compilation_string.Replace("/* OTHER FUNCTIONS */", otherFunction);

            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters compiler_parameters = new CompilerParameters(references.ToArray());
            compiler_parameters.GenerateInMemory = true;
            compiler_parameters.GenerateExecutable = true;
            CompilerResults results = provider.CompileAssemblyFromSource(compiler_parameters, compilation_string);

            // Check errors
            if (results.Errors.HasErrors)
            {
                StringBuilder builder = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    builder.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }
                throw new InvalidOperationException(builder.ToString());
            }

            // Execute
            Assembly assembly = results.CompiledAssembly;
            Type program = assembly.GetType("RuntimeCompilationScriptTestCode");

            PropertyInfo driverX = program.GetProperty("Driver");
            driverX.SetValue(null, driver);

            MethodInfo[] methods = program.GetMethods();

            foreach (MethodInfo m in methods)
            {
                //execute all method named ???test
                if (m.Name.ToLower().Contains("_test"))
                {
                    m.Invoke(null, null);
                }
            }

            MethodInfo executeScript = program.GetMethod("ExecuteScript");
            //return executeScript.Invoke(null, null);
        }


        public static string GenerateVisualStudioProjectTest(DeviceConfig deviceConfigAndroid, DeviceConfig deviceConfigIOS, TpLocatorEstrategy locatorEstrategy, XPathSelector.XPathType individualExpressionType)
        {

            string dirProjectTemplate = Path.GetDirectoryName(Application.ExecutablePath) + @"\Resources\UnitTestProjectTemplate";

            string dirOutPut = Path.GetDirectoryName(Application.ExecutablePath) + @"\output";

            if (!Directory.Exists(dirOutPut))
                Directory.CreateDirectory(dirOutPut);

            string output = dirOutPut + @"\UnitTestProject-" + DateTime.Now.ToString("yyyy-MM-dd HHmmss");

            if (_onlyTestClass)
                output += "-TestClass";

            string appTestName = deviceConfigAndroid.TestCaseName.Replace(".", "").Replace("-", "").Replace(" ", "");

            string appTest = "";

            StringBuilder script = new StringBuilder();
            string nl = Environment.NewLine;
            string functionCall;
            string appiumConfig;

            string nameSpace = appTestName;
            string className = appTestName + locatorEstrategy.ToString();
            if (locatorEstrategy == TpLocatorEstrategy.IndividualExpression)
                className += individualExpressionType.ToString();

            if (!_onlyTestClass)
            {
                Utils.DirectoryCopy(dirProjectTemplate, output);

                File.Move(output + @"\Properties\AssemblyInfo.txt", output + @"\Properties\AssemblyInfo.cs");
                appTest = File.ReadAllText(output + @"\AppTest.txt");
                string unitProjectTest = File.ReadAllText(output + @"\UnitTestProject.csproj");
                string projectConfig = File.ReadAllText(output + @"\ProjectConfig.txt");
                string locatorStrategy = File.ReadAllText(output + @"\LocatorStrategy.txt");

                unitProjectTest = unitProjectTest.Replace("@AppTest", className + ".cs");
                                 
                File.WriteAllText(output + @"\ProjectConfig.cs", projectConfig);
                File.Delete(output + @"\ProjectConfig.txt");

                unitProjectTest = unitProjectTest.Replace("AppTest", appTestName);

                File.WriteAllText(output + @"\UnitTestProject.csproj", unitProjectTest);

                File.WriteAllText(output + @"\LocatorStrategy.cs", locatorStrategy);
                File.Delete(output + @"\LocatorStrategy.txt");

            }
            else
            {
                appTest = File.ReadAllText(dirProjectTemplate + @"\AppTest.txt");

                Directory.CreateDirectory(output);
            }

            script.AppendLine(ScriptGenerate.GenerateScriptMethodTest(deviceConfigAndroid, deviceConfigIOS, locatorEstrategy, individualExpressionType, false, "_driver", out functionCall, out appiumConfig));



            
            appTest = appTest.Replace("@Namespace", nameSpace);
            appTest = appTest.Replace("@ClassName", className);
            appTest = appTest.Replace("/*REPLACE: appium config*/", appiumConfig);
            appTest = appTest.Replace("/*REPLACE: call sequence*/", functionCall);
            appTest = appTest.Replace("/*REPLACE: script*/", script.ToString());

            File.WriteAllText(output + @"\" + className + ".cs", appTest);
            File.Delete(output + @"\AppTest.txt");

            return output;

        }

    }
}
