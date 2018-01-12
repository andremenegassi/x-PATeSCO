using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Tesseract;

namespace CrossPlatformCompatibility.Support
{
  
    public class MyNode
    {

        public static string[] AndroidKeyAttributes
        {
            get { return new string[] { "resource-id", "content-desc", "text" }; }
        }

        public static string AndroidHybridContainer
        {
            get { return "android.webkit.WebView"; }
        }

        public static string[] IOSKeyAttributes
        {
            get { return new string[] { "name", "label", "value" }; }
        }

        public static string IOSHybridContainer
        {
            get { return "UIAWebView"; }
        }

        public struct NodeCompare
        {
            public string WordInNode { get; set; }
            public int Qtd { get; set; }
        }

        public struct TpGUIScreenshot
        {
            [JsonConverter(typeof(CustomBitmapConverter))]
            public Image Screenshot { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

        }

        public class TpExecutationState
        {
            public bool Executed { get; set; }
            public double Runtime { get; set; }
            public XDocument Xml { get; set; }
            public TpGUIScreenshot GUIScreenshot { get; set; }
            
            public TpVerificationStateCriteria VerificationStateCriteria { get; set; }

            public TpExecutationState()
            {
                VerificationStateCriteria = new TpVerificationStateCriteria();
            }

        }

        public class TpVerificationStateCriteria
        {
            public string DeviceCRefName { get; set; }

            /*CompatibilityCriteria*/
            public double CompatibilityCriteriaGUIScreenshotSimilarityPercentage { get; set; }
            public double CompatibilityCriteriaGUIScreenshotOCRSimilarityPercentage { get; set; }
            public double CompatibilityCriteriaTextSimilarityHybridPercentage { get; set; }
            public double CompatibilityCriteriaTextSimilarityTotalPercentage { get; set; }
            public double CompatibilityCriteriaRuntimeDifferencePercentage { get; set; }
            public bool CompatibilityCriteriaRuntimeFaster { get; set; }

            /*EquivalenceCriteria*/
            public bool EquivalenceCriteriaElementNegativelyPositioning { get; set; }

        }


        public string Platform { get; set; }
        public XElement Element { get; set; }
        [JsonIgnore]
        public string ElementType { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> KeyAttributes { get; set; }

        public float ElementPositionX { get; set; }
        public float ElementPositionY { get; set; }
        public float ElementWidth { get; set; }
        public float ElementHeight{ get; set; }
        public string EventName { get; set; }
        public bool ElementCrossPlatformValid { get; set; }

        [JsonIgnore]
        public List<string> ElementCrossPlatformSelector { get; set; }
        public string ElementXPathSelector { get; set; }
        public bool SelectedForTest { get; set; }
        public bool SendClick { get; set; }
        public bool SendKeys { get; set; }
        public string SendKeysText { get; set; }

        public int WaitElementBySecond { get; set; }
        public TpGUIScreenshot GUIScreenshot { get; set; }

        public TpExecutationState ExecutationState { get; set; }



        public MyNode()
        {
            ElementCrossPlatformSelector = new List<string>();
            ExecutationState = new TpExecutationState();
        }


        public static string ExtractDeviceId(DeviceConfig deviceConfig)
        {
            return deviceConfig.Server + " " +
                   deviceConfig.PlatformName + " " +
                   deviceConfig.PlatformVersion + " " +
                   deviceConfig.DeviceName;
        }

        public static void ExtractElementDimension (MyNode node, out float x, out float y, out float w, out float h)
        {

            x = 0; y = 0; w = 0; h = 0;

            if (node.Platform == "Android")
            {
                if (node.Element.Attribute("bounds") == null)
                    return;

                string bounds = node.Element.Attribute("bounds").Value; //"[x,y][w,h]"

                if (bounds != "")
                {
                    string[] location = bounds.Replace("][", ",").Replace("[", "").Replace("]", "").Split(',');

                    //x = int.Parse(location[0]) > 0 ? int.Parse(location[0]) : 0;
                    //y = int.Parse(location[1]) > 0 ? int.Parse(location[1]) : 0;

                    x = int.Parse(location[0]);
                    y = int.Parse(location[1]);

                    w = int.Parse(location[2]) > 0 ? int.Parse(location[2]) : 0;

                    if (x > 0)
                        w = w - x;

                    h = int.Parse(location[3]) > 0 ? int.Parse(location[3]) : 0;

                    if (y > 0)
                        h = h - y;
                }

            }
            else {

                if (node.Element.Attribute("x") == null || node.Element.Attribute("y") == null || node.Element.Attribute("width") == null || node.Element.Attribute("height") == null)
                    return;

                x = Convert.ToInt32(Convert.ToDouble(node.Element.Attribute("x").Value));
                y = Convert.ToInt32(Convert.ToDouble(node.Element.Attribute("y").Value));
                w = Convert.ToInt32(Convert.ToDouble(node.Element.Attribute("width").Value));
                h = Convert.ToInt32(Convert.ToDouble(node.Element.Attribute("height").Value));
            }


        }


        public static void CompareStates(MyNode nodeCref, MyNode nodeCn, bool compareTextSimilarity, bool compareScreenshotSimilarity, bool compareScreenshotOCRSimilarity)
        {
            if (!nodeCref.ExecutationState.Executed && !nodeCn.ExecutationState.Executed)
            {
                return;
            }

            nodeCref.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaElementNegativelyPositioning = false;
            nodeCn.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaElementNegativelyPositioning = false;

            if (nodeCref.ElementPositionX < 0 || nodeCref.ElementPositionY < 0)
            {
                nodeCref.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaElementNegativelyPositioning = true;
            }

            if (nodeCn.ElementPositionX < 0 || nodeCn.ElementPositionY < 0)
            { 
                nodeCn.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaElementNegativelyPositioning = true;
            }

            if (compareTextSimilarity)
                ExtractTextSimilarity(nodeCref, nodeCn);

            if (compareScreenshotSimilarity)
                ExtractScreenshotSimilarity(nodeCref, nodeCn);

            if (compareScreenshotOCRSimilarity)
                ExtractScreenshotOCRSimilarity(nodeCref, nodeCn);

            ExtractRuntimeFasterDifference(nodeCref, nodeCn);
        }

    

        private static void ExtractTextSimilarity(MyNode nodeCref, MyNode nodeCn)
        {
            if (!nodeCref.ExecutationState.Executed && !nodeCn.ExecutationState.Executed)
            {
                return;
            }
            string[] attrs1 = null;
            string[] attrs2 = null;
            string containerHybrid1 = "";
            string containerHybrid2 = "";

            //device 1
            if (nodeCref.Platform == "Android")
            {
                attrs1 = MyNode.AndroidKeyAttributes;
                containerHybrid1 = MyNode.AndroidHybridContainer;
            }
            else //IOS
            {
                attrs1 = MyNode.IOSKeyAttributes;
                containerHybrid1 = MyNode.IOSHybridContainer;
            }

            //device 2
            if (nodeCn.Platform == "Android")
            {
                attrs2 = MyNode.AndroidKeyAttributes;
                containerHybrid2 = MyNode.AndroidHybridContainer;

            }
            else //IOS
            {
                attrs2 = MyNode.IOSKeyAttributes;
                containerHybrid2 = MyNode.IOSHybridContainer;

            }

            #region device 1
            List<NodeCompare> attrsValues1Hybrid = ExtractNodeCompare(nodeCref, attrs1, containerHybrid1);
            List<NodeCompare> attrsValues1 = ExtractNodeCompare(nodeCref, attrs1, "");
            #endregion

            #region device 2
            List<NodeCompare> attrsValues2Hybrid = ExtractNodeCompare(nodeCn, attrs2, containerHybrid2);
            List<NodeCompare> attrsValues2 = ExtractNodeCompare(nodeCn, attrs2, "");
            #endregion

            double similarity = 0;
            double similarityAverage = 0;


            #region finding similarity

            #region  nodes hybrid device 2 vs device 1

            foreach (NodeCompare n2 in attrsValues2Hybrid)
            {
                foreach (NodeCompare n1 in attrsValues1Hybrid)
                {
                    if (n1.WordInNode == n2.WordInNode && n1.Qtd == n2.Qtd)
                    {
                        similarity++;
                    }
                }
            }
            
            similarityAverage = (similarity * 100) / attrsValues2Hybrid.Count;
            nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaTextSimilarityHybridPercentage = similarityAverage;
            #endregion


            #region all nodes device 2 vs device 1
            similarity = 0;

            foreach (NodeCompare n2 in attrsValues2)
            {
                foreach (NodeCompare n1 in attrsValues1)
                {
                    if (n1.WordInNode == n2.WordInNode && n1.Qtd == n2.Qtd)
                    {
                        similarity++;
                    }

                }
            }

            similarityAverage = (similarity * 100) / attrsValues2.Count;
            nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaTextSimilarityTotalPercentage = similarityAverage;
            #endregion

  
            #endregion
        }

        private static List<NodeCompare> ExtractNodeCompare(MyNode node, string[] attrs, string containerXML)
        {
            List<NodeCompare> attrsValues = new List<NodeCompare>();

            IEnumerable<XElement> elements;

            if (!string.IsNullOrEmpty(containerXML))
                elements = node.ExecutationState.Xml.Descendants(containerXML).Descendants().Distinct();
            else elements = node.ExecutationState.Xml.Descendants().Distinct();

            foreach (XElement e in elements)
            {
                for (int i = 0; i < attrs.Length; i++)
                {
                    if (e.Attribute(attrs[i]) != null && e.Attribute(attrs[i]).Value.Trim() != "")
                    {
                        string[] words = e.Attribute(attrs[i]).Value.Trim().Split(' ').Select(w => w.ToLower()).ToArray();

                        foreach (string word in words)
                        {
                            if (!string.IsNullOrEmpty(word.Trim()))
                            {
                                NodeCompare n = attrsValues.Find(w => w.WordInNode == word);

                                if (n.Qtd > 0)
                                {
                                    n.Qtd++;
                                }
                                else
                                {
                                    NodeCompare v = new NodeCompare()
                                    {
                                        WordInNode = word,
                                        Qtd = 1
                                    };

                                    attrsValues.Add(v);
                                }
                            }
                        }

                    }
                }
            }

            return attrsValues;
        }


        private static void ExtractScreenshotSimilarity(MyNode nodeCref, MyNode nodeCn)
        {
            Image screenshotCref = nodeCref.ExecutationState.GUIScreenshot.Screenshot;
            Image screenshotCn = nodeCn.ExecutationState.GUIScreenshot.Screenshot;

            int w, h;

            if (screenshotCref.Width < screenshotCn.Width)
                w = screenshotCref.Width;
            else w = screenshotCn.Width;

            if (screenshotCref.Height < screenshotCn.Height)
                h = screenshotCref.Height;
            else h = screenshotCn.Height;

            if (screenshotCref.Width != w  && screenshotCref.Height != h)
            {
                screenshotCref = ImageUtils.Scale(screenshotCref, w, h);
            }

            if (screenshotCn.Width != w && screenshotCn.Height != h)
            {
                screenshotCn = ImageUtils.Scale(screenshotCn, w, h);
            }

            double diffCnVsCref = ImageUtils.Similarity(screenshotCn, screenshotCref);

            nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaGUIScreenshotSimilarityPercentage = diffCnVsCref;
        }


        private static void ExtractScreenshotOCRSimilarity(MyNode nodeCref, MyNode nodeCn)
        {
            //Device1
            string ocr1 = ImageUtils.ExtractOCR(nodeCref.ExecutationState.GUIScreenshot.Screenshot);

            //Device2
            string ocr2 = ImageUtils.ExtractOCR(nodeCn.ExecutationState.GUIScreenshot.Screenshot);

            double similarityCnVsCref = Levenshtein.CalculateSimilarityPercentage(ocr2, ocr1);

            nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaGUIScreenshotOCRSimilarityPercentage = similarityCnVsCref;
        }

        private static void ExtractRuntimeFasterDifference(MyNode nodeCref, MyNode nodeCn)
        {
            //Device1
            double d1 = nodeCref.ExecutationState.Runtime;

            //Device2
            double d2 = nodeCn.ExecutationState.Runtime;

            nodeCref.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeFaster = d1 <= d2;
            nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeFaster = d2 <= d1;

            if (nodeCref.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeFaster)
            {
                double percent = ((d2 - d1) / d1) * 100;
                nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeDifferencePercentage = percent;
            }
            else
            {
                double percent = ((d1 - d2) / d2) * 100;
                nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeDifferencePercentage = percent;
            }

        }


        public static bool CompareStates(DeviceConfig cref, DeviceConfig cn, bool compareTextSimilarity, bool compareScreenshotSimilarity, bool compareScreenshotOCRSimilarity, out string msg)
        {
            bool ok = false;
            msg = "";

            if (CheckDeviceConfigCompatibility(cref, cn, out msg))
            {
                int qtd = cn.Events.Count;

                for (int i = 0; i < qtd; i++)
                {
                    Support.MyNode.CompareStates(cref.Events[i], cn.Events[i], compareTextSimilarity, compareScreenshotSimilarity, compareScreenshotOCRSimilarity);
                }

                ok = true;
            }

            return ok;
        }


        public static bool CheckDeviceConfigCompatibility(DeviceConfig cref, DeviceConfig cn, out string msg)
        {
            msg = "";

            if (ExtractDeviceId(cref).Trim() == "" || ExtractDeviceId(cn).Trim() == "")
            {
                msg = "Devices not configured.";
                return false;
            }

            if (cn.Events == null)
            {
                msg = ExtractDeviceId(cn) + " sequence event not defined.";
                return false;
            }

            if (cref.Events == null)
            {
                msg = ExtractDeviceId(cref) + " sequence event not defined.";
                return false;
            }

            if (cn.Events == null)
            {
                msg = ExtractDeviceId(cn) + " sequence event not defined.";
                return false;
            }


            if (cref.Events.Count != cn.Events.Count)
            {
                msg = "The amount of events is incompatible.";
                return false;
            }

            foreach (MyNode n in cref.Events)
            {
                if (n.ExecutationState.Equals(null))
                {
                    msg = "Executation state undefined. Run test (" + ExtractDeviceId(cref) +  ").";
                    return false;
                }
            }

            foreach (MyNode n in cn.Events)
            {
                if (n.ExecutationState.Equals(null))
                {
                    msg = "Executation state undefined. Run test (" + ExtractDeviceId(cn) + ").";
                    return false;
                }
            }

            int qtd = cref.Events.Count;

            for (int i = 0; i < qtd; i++)
            {
                if (cref.Events[i].EventName != cn.Events[i].EventName)
                {
                    msg = "EventName "  + (i + 1) + " - '" + cref.Events[i].EventName + "' and '" + cn.Events[i].EventName + "' are different.";
                    return false;
                }
                else if(cref.Events[i].SendClick != cn.Events[i].SendClick)
                {
                    msg = "SendClick " + (i + 1) + " - '" + cref.Events[i].EventName + "' and '" + cn.Events[i].EventName + "' are different.";
                    return false;
                }
                else if (cref.Events[i].SendKeysText != cn.Events[i].SendKeysText)
                {
                    msg = "SendKeysText " + (i + 1) + " - '" + cref.Events[i].EventName + "' and '" + cn.Events[i].EventName + "' are different.";
                    return false;
                }
                else if (cref.Events[i].WaitElementBySecond != cn.Events[i].WaitElementBySecond)
                {
                    msg = "WaitElementBySecond " + (i + 1) + " - '" + cref.Events[i].EventName + "' and '" + cn.Events[i].EventName + "' are different.";
                    return false;
                }
               
            }


            return true;

        }


        public static string CompareStatesGenerateOutPut(DeviceConfig cref, DeviceConfig cn, bool compareTextSimilarity, bool compareScreenshotSimilarity, bool compareScreenshotOCRSimilarity)
        {
       
            string html = File.ReadAllText("Resources/template2.html");

            string templateNodes = @"<div class=""connector""></div>
                                     <div class=""event"">
                                         <div class=""eventId"">{0}</div>
                                     </div>";

            string templateComparisonNodes = @"<div class=""connector""></div>
                                               <div class=""event eventComparison"">
                                                    <div class=""titleCriteria"">
                                                        COMPATIBILITY CRITERIA
                                                    </div>
                                                    <b>*TSHWVC:</b> {0}%
                                                    <br>
                                                    <b>*TST:</b> {1}% 
                                                    <br>
                                                    <b>*SS:</b> {2}% 
                                                    <br>
                                                    <b>*SOCRS:</b> {3}% 
                                                    <br>
                                                    <b>*Runtime:</b> {4}% ({5} is faster) 
                                                    <br>    
                                                    <br>
                                                    <div class=""titleCriteria"">
                                                        EQUIVALENCE CRITERIA
                                                    </div>
                                                    <b>*ENP:</b> {6} <br>
                                               </div>";


            List<string> nodes = new List<string>();

            int i = 0;
           
            html = html.Replace("@deviceName1", MyNode.ExtractDeviceId(cref));
            nodes.Clear();
            i = 0;

            //device2 vs device1
            foreach (MyNode n in cn.Events)
            {
                string eventName = n.EventName;

                if (string.IsNullOrEmpty(eventName))
                    eventName = "unnamed";

                if (eventName.Length > 20)
                {
                    eventName = eventName.Substring(0, 20);
                }

                nodes.Add(string.Format(templateNodes, eventName));

                i++;
            }

            html = html.Replace("@deviceName2", MyNode.ExtractDeviceId(cn));
            html = html.Replace("@devicesModel", string.Join(Environment.NewLine, nodes.ToArray()));


            #region Comparasion device 2 vs device 1
            List<string> nodesCompare = new List<string>();
            foreach (MyNode n in cn.Events)
            {
                string textSimilarityHybridPercentage = "-";
                string textSimilarityTotalPercentage = "-";
                string guiScreenshotSimilarityPercentage = "-";
                string guiScreenshotOCRSimilarityPercentage = "-";
                string runtimeDifferencePercentage = "-";
                string runtimeFaster = "-";
                string elementNegativelyPositioning = "-";
                
                if (compareTextSimilarity)
                {
                    textSimilarityHybridPercentage = Math.Round(n.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaTextSimilarityHybridPercentage, 2).ToString();
                    textSimilarityTotalPercentage = Math.Round(n.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaTextSimilarityTotalPercentage, 2).ToString();
                }

                if (compareScreenshotSimilarity)
                {
                    guiScreenshotSimilarityPercentage = Math.Round(n.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaGUIScreenshotSimilarityPercentage, 2).ToString();
                }

                if (compareScreenshotOCRSimilarity)
                {
                    guiScreenshotOCRSimilarityPercentage = Math.Round(n.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaGUIScreenshotOCRSimilarityPercentage, 2).ToString();
                }

                if (n.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeFaster)
                    runtimeFaster = "Cn";
                else runtimeFaster = "Cref";


                runtimeDifferencePercentage = Math.Round(n.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeDifferencePercentage, 2).ToString();


                elementNegativelyPositioning = n.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaElementNegativelyPositioning.ToString();

                nodesCompare.Add(string.Format(templateComparisonNodes,
                    textSimilarityHybridPercentage,
                    textSimilarityTotalPercentage,
                    guiScreenshotSimilarityPercentage,
                    guiScreenshotOCRSimilarityPercentage,
                    runtimeDifferencePercentage,
                    runtimeFaster,
                    elementNegativelyPositioning));
            }

            html = html.Replace("@comparison2vs1", string.Join(Environment.NewLine, nodesCompare.ToArray()));
            #endregion


            string dirOutPut = Path.GetDirectoryName(Application.ExecutablePath) + @"\output";

            if (!Directory.Exists(dirOutPut))
                Directory.CreateDirectory(dirOutPut);


            string output = dirOutPut + @"\" + DateTime.Now.ToString("yyyy-MM-dd HHmss") + ".html";

            TextWriter tw = new StreamWriter(output, true);
            tw.WriteLine(html);
            tw.Close();

            return output;
            
        }


        public static string ExtractCrossPlatformSelector(MyNode nodeAndroid, MyNode nodeIOS)
        {

            string xPath = string.Format("//*[@content-desc='{0}' or @text='{1}' or @resource-id='{2}' or @content-desc='{3}' or @text='{4}' or @resource-id='{5}']", "search", "search", "search", "Search For a Market", "Search For a Market", "Search For a Market");


            List<string> selectors = new List<string>();

            //Android

            if (nodeAndroid.Element.Attribute("resource-id") != null && nodeAndroid.Element.Attribute("resource-id").Value != "")
            {
                selectors.Add("@resource-id='" + nodeAndroid.Element.Attribute("resource-id").Value +  "'");
            }
            else if (nodeAndroid.Element.Attribute("content-desc") != null && nodeAndroid.Element.Attribute("content-desc").Value != "")
            {
                selectors.Add("@content-desc='" + nodeAndroid.Element.Attribute("content-desc").Value + "'");
            }
            else if (nodeAndroid.Element.Attribute("text") != null && nodeAndroid.Element.Attribute("text").Value != "")
            {
                selectors.Add("@text='" + nodeAndroid.Element.Attribute("text").Value + "'");
            }

            //iOS

            if (nodeIOS.Element.Attribute("name") != null && nodeIOS.Element.Attribute("name").Value != "")
            {
                selectors.Add("@name='" + nodeIOS.Element.Attribute("name").Value + "'");
            }
            else if (nodeIOS.Element.Attribute("label") != null && nodeIOS.Element.Attribute("label").Value != "")
            {
                selectors.Add("@label='" + nodeIOS.Element.Attribute("labe").Value + "'");
            }
            else if (nodeIOS.Element.Attribute("value") != null && nodeIOS.Element.Attribute("value").Value != "")
            {
                selectors.Add("@value='" + nodeIOS.Element.Attribute("value").Value + "'");
            }

             
            return "//*[" + string.Join(" or ", selectors.ToArray()) + "]";

        }


    }

}
