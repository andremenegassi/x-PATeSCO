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
using System.Xml.XPath;
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
            public double RuntimePercent { get; set; }
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
            //public double CompatibilityCriteriaRuntimePercentage { get; set; }
            public bool CompatibilityCriteriaRuntimeFaster { get; set; }
            public bool CompatibilityCriteriaElementNegativelyPositioning { get; set; }


            /*EquivalenceCriteria*/
            public bool EquivalenceCriteriaCompareAttributesValues { get; set; }

        }


        public string Platform { get; set; }
        public XElement Element { get; set; }
        public string ElementType { get; set; }
        public string XMLDoc { get; set; }
        public Dictionary<string, string> KeyAttributes { get; set; }
        public float ElementPositionX { get; set; }
        public float ElementPositionY { get; set; }
        public float ElementWidth { get; set; }
        public float ElementHeight{ get; set; }
        public string EventName { get; set; }
        public string ElementXPathAbsoluteSelector { get; set; }
        public bool SendClick { get; set; }
        public bool SendKeys { get; set; }
        public string SendKeysText { get; set; }
        public int WaitElementBySecond { get; set; }
        public TpGUIScreenshot GUIScreenshot { get; set; }
        public TpExecutationState ExecutationState { get; set; }

        public MyNode()
        {
            ExecutationState = new TpExecutationState();
        }

        public static List<XPathSelector> ExtractSelectors(MyNode node)
        {
            if (node.Platform == "Android")
               return ExtractSelectors(node, null);
            else if (node.Platform == "iOS")
               return ExtractSelectors(null, node);

            return null;
        }

        public static List<XPathSelector> ExtractSelectors(MyNode nodeAndroid, MyNode nodeIOS) {

            List<XPathSelector> selectors = new List<XPathSelector>();

            #region 1 AbsolutePath
            XPathSelector xpath = new XPathSelector();
            xpath.Type = XPathSelector.XPathType.AbsolutePath;

            if (nodeAndroid != null)
            {
                xpath.XPathForAndroid = nodeAndroid.ElementXPathAbsoluteSelector;
            }
            if (nodeIOS != null)
            {
                xpath.XPathForIOS = nodeIOS.ElementXPathAbsoluteSelector;
            }
            selectors.Add(xpath);
            #endregion

            #region 2  IdentifyAttributes
            xpath = new XPathSelector();
            xpath.Type = XPathSelector.XPathType.IdentifyAttributes;

            //Android
            if (nodeAndroid != null)
            {
                string attr = "resource-id";

                if (nodeAndroid.KeyAttributes != null && nodeAndroid.KeyAttributes.ContainsKey(attr))
                {
                    string attrValue = nodeAndroid.KeyAttributes[attr];

                    if (!string.IsNullOrEmpty(attrValue))
                    {
                        xpath.XPathForAndroid = "//*[@" + attr + "='" + attrValue + "']";
                    }
                }
            }

            //iOS
            if (nodeIOS != null)
            {
                string attr = "name";

                if (nodeIOS.KeyAttributes != null && nodeIOS.KeyAttributes.ContainsKey(attr))
                {
                    string attrValue = nodeIOS.KeyAttributes[attr];

                    if (!string.IsNullOrEmpty(attrValue))
                    {
                        xpath.XPathForIOS = "//*[@" + attr + "='" + attrValue + "']";
                    }
                }
            }

            selectors.Add(xpath);

            #endregion;

            #region 3 CrossPlatform
            xpath = new XPathSelector();
            xpath.Type = XPathSelector.XPathType.CrossPlatform;

            List<string> aux = new List<string>();


            //Android
            if (nodeAndroid != null)
            {
                if (nodeAndroid.Element.Attribute("content-desc") != null && nodeAndroid.Element.Attribute("content-desc").Value != "")
                {
                    aux.Add("@content-desc='" + CleanValueAttributeToSelector(nodeAndroid.Element.Attribute("content-desc").Value) + "'");
                }
                else if (nodeAndroid.Element.Attribute("text") != null && nodeAndroid.Element.Attribute("text").Value != "")
                {
                    aux.Add("@text='" + CleanValueAttributeToSelector(nodeAndroid.Element.Attribute("text").Value) + "'");
                }
            }
          
            //iOS
            if (nodeIOS != null)
            {
                if (nodeIOS.Element.Attribute("label") != null && nodeIOS.Element.Attribute("label").Value != "")
                {
                    aux.Add("@label='" + CleanValueAttributeToSelector(nodeIOS.Element.Attribute("label").Value) + "'");
                }
                else if (nodeIOS.Element.Attribute("value") != null && nodeIOS.Element.Attribute("value").Value != "")
                {
                    aux.Add("@value='" + CleanValueAttributeToSelector(nodeIOS.Element.Attribute("value").Value) + "'");
                }
            }

            if (aux.Count > 0)
            {
                xpath.XPathForAndroid = "//*[" + string.Join(" or ", aux.ToArray()) + "]";

                xpath.XPathForIOS = "//*[" + string.Join(" or ", aux.ToArray()) + "]";
            }
            

            selectors.Add(xpath);

            #endregion

            #region 4 ElementType
            xpath = new XPathSelector();
            xpath.Type = XPathSelector.XPathType.ElementType;

            if (nodeAndroid != null)
            {
                aux.Clear();
                if (nodeAndroid.Element.Attribute("content-desc") != null && nodeAndroid.Element.Attribute("content-desc").Value != "")
                {
                    aux.Add("@content-desc='" + CleanValueAttributeToSelector(nodeAndroid.Element.Attribute("content-desc").Value) + "'");
                }
                else if (nodeAndroid.Element.Attribute("text") != null && nodeAndroid.Element.Attribute("text").Value != "")
                {
                    aux.Add("@text='" + CleanValueAttributeToSelector(nodeAndroid.Element.Attribute("text").Value) + "'");
                }
                else if (nodeAndroid.Element.Attribute("resource-id") != null && nodeAndroid.Element.Attribute("resource-id").Value != "")
                {
                    aux.Add("@resource-id='" + nodeAndroid.Element.Attribute("resource-id").Value + "'");
                }
                if (aux.Count > 0)
                    xpath.XPathForAndroid = "//*/" + nodeAndroid.ElementType + "[" + string.Join(" and ", aux.ToArray()) + "]";
            }

            
            if (nodeIOS != null)
            {
                aux.Clear();
                if (nodeIOS.Element.Attribute("label") != null && nodeIOS.Element.Attribute("label").Value != "")
                {
                    aux.Add("@label='" + CleanValueAttributeToSelector(nodeIOS.Element.Attribute("label").Value) + "'");
                }
                else if (nodeIOS.Element.Attribute("value") != null && nodeIOS.Element.Attribute("value").Value != "")
                {
                    aux.Add("@value='" + CleanValueAttributeToSelector(nodeIOS.Element.Attribute("value").Value) + "'");
                }
                else if (nodeIOS.Element.Attribute("name") != null && nodeIOS.Element.Attribute("name").Value != "")
                {
                    aux.Add("@name='" + nodeIOS.Element.Attribute("name").Value + "'");
                }
                if (aux.Count > 0)
                    xpath.XPathForIOS = "//*/" + nodeIOS.ElementType + "[" + string.Join(" and ", aux.ToArray()) + "]";

            }

            selectors.Add(xpath);
            #endregion

            #region 5 AncestorIndex
            xpath = new XPathSelector();
            xpath.Type = XPathSelector.XPathType.AncestorIndex;


            if (nodeAndroid != null)
            {
                List<string> auxElementParent = new List<string>();

                XDocument xmlDoc = XDocument.Parse(nodeAndroid.XMLDoc);

                XElement element = xmlDoc.XPathSelectElement(nodeAndroid.ElementXPathAbsoluteSelector);
                XElement elementParent = element.Parent;

                List<string> indexes = new List<string>();

                while (elementParent != null && auxElementParent.Count == 0)
                {
                    indexes.Add("/*[" + XMLUtils.GetIndexInParent(element, elementParent) + "]");

                    if (elementParent.Attribute("content-desc") != null && elementParent.Attribute("content-desc").Value != "")
                    {
                        auxElementParent.Add("@content-desc='" + CleanValueAttributeToSelector(elementParent.Attribute("content-desc").Value) + "'");
                    }
                    else if (elementParent.Attribute("text") != null && elementParent.Attribute("text").Value != "")
                    {
                        auxElementParent.Add("@text='" + CleanValueAttributeToSelector(elementParent.Attribute("text").Value) + "'");
                    }
                    else if (elementParent.Attribute("resource-id") != null && elementParent.Attribute("resource-id").Value != "")
                    {
                        auxElementParent.Add("@resource-id='" + elementParent.Attribute("resource-id").Value + "'");
                    }

                    element = elementParent;
                    elementParent = elementParent.Parent;
                }

                if (auxElementParent.Count > 0)
                {
                    indexes.Reverse();
                    xpath.XPathForAndroid = "//*[" + string.Join(" and ", auxElementParent.ToArray()) + "]" + string.Join("", indexes);
                }
            }


            if (nodeIOS != null)
            {
                List<string> auxElementParent = new List<string>();

                XDocument xmlDoc = XDocument.Parse(nodeIOS.XMLDoc);

                XElement element = xmlDoc.XPathSelectElement(nodeIOS.ElementXPathAbsoluteSelector);
                XElement elementParent = element.Parent;

                List<string> indexes = new List<string>();

                while (elementParent != null && auxElementParent.Count == 0)
                {
                    indexes.Add("/*[" + XMLUtils.GetIndexInParent(element, elementParent) + "]");

                    if (elementParent.Attribute("label") != null && elementParent.Attribute("label").Value != "")
                    {
                        auxElementParent.Add("@label='" + CleanValueAttributeToSelector(elementParent.Attribute("label").Value) + "'");
                    }
                    else if (elementParent.Attribute("value") != null && elementParent.Attribute("value").Value != "")
                    {
                        auxElementParent.Add("@value='" + CleanValueAttributeToSelector(elementParent.Attribute("value").Value) + "'");
                    }
                    else if (elementParent.Attribute("name") != null && elementParent.Attribute("name").Value != "")
                    {
                        auxElementParent.Add("@name='" + elementParent.Attribute("name").Value + "'");
                    }

                    element = elementParent;
                    elementParent = elementParent.Parent;
                }


                if (auxElementParent.Count > 0)
                {

                    indexes.Reverse();

                    xpath.XPathForIOS = "//*[" + string.Join(" and ", auxElementParent.ToArray()) + "]" + string.Join("", indexes);

                }
            }

            selectors.Add(xpath);

            #endregion

            #region 6 AncestorAttributes
            xpath = new XPathSelector();
            xpath.Type = XPathSelector.XPathType.AncestorAttributes;

            if (nodeAndroid != null)
            {
                XDocument xmlDoc = XDocument.Parse(nodeAndroid.XMLDoc);

                XElement element = xmlDoc.XPathSelectElement(nodeAndroid.ElementXPathAbsoluteSelector);
                XElement elementParent = element.Parent;

                List<string> auxElementParent = new List<string>();

                while (elementParent != null && auxElementParent.Count == 0)
                {
                    if (elementParent.Attribute("content-desc") != null && elementParent.Attribute("content-desc").Value != "")
                    {
                        auxElementParent.Add("@content-desc='" + CleanValueAttributeToSelector(elementParent.Attribute("content-desc").Value) + "'");
                    }
                    else if (elementParent.Attribute("text") != null && elementParent.Attribute("text").Value != "")
                    {
                        auxElementParent.Add("@text='" + CleanValueAttributeToSelector(elementParent.Attribute("text").Value) + "'");
                    }
                    else if (elementParent.Attribute("resource-id") != null && elementParent.Attribute("resource-id").Value != "")
                    {
                        auxElementParent.Add("@resource-id='" + elementParent.Attribute("resource-id").Value + "'");
                    }

                    element = elementParent;
                    elementParent = elementParent.Parent;
                }


                aux.Clear();
                if (nodeAndroid.Element.Attribute("content-desc") != null && nodeAndroid.Element.Attribute("content-desc").Value != "")
                {
                    aux.Add("@content-desc='" + CleanValueAttributeToSelector(nodeAndroid.Element.Attribute("content-desc").Value) + "'");
                }
                else if (nodeAndroid.Element.Attribute("text") != null && nodeAndroid.Element.Attribute("text").Value != "")
                {
                    aux.Add("@text='" + CleanValueAttributeToSelector(nodeAndroid.Element.Attribute("text").Value) + "'");
                }
                else if (nodeAndroid.Element.Attribute("resource-id") != null && nodeAndroid.Element.Attribute("resource-id").Value != "")
                {
                    aux.Add("@resource-id='" + nodeAndroid.Element.Attribute("resource-id").Value + "'");
                }

                if (auxElementParent.Count > 0 && aux.Count > 0)
                    xpath.XPathForAndroid = "//*[" + string.Join(" and ", auxElementParent.ToArray()) + "]//*[" + string.Join(" and ", aux.ToArray()) + "]";
            }


            if (nodeIOS != null)
            {
                XDocument xmlDoc = XDocument.Parse(nodeIOS.XMLDoc);

                XElement element = xmlDoc.XPathSelectElement(nodeIOS.ElementXPathAbsoluteSelector);
                XElement elementParent = element.Parent;

                List<string> auxElementParent = new List<string>();

                while (elementParent != null && auxElementParent.Count == 0)
                {
                    if (elementParent.Attribute("label") != null && elementParent.Attribute("label").Value != "")
                    {
                        auxElementParent.Add("@label='" + CleanValueAttributeToSelector(elementParent.Attribute("label").Value) + "'");
                    }
                    else if (elementParent.Attribute("value") != null && elementParent.Attribute("value").Value != "")
                    {
                        auxElementParent.Add("@value='" + CleanValueAttributeToSelector(elementParent.Attribute("value").Value) + "'");
                    }
                    else if (elementParent.Attribute("name") != null && elementParent.Attribute("name").Value != "")
                    {
                        auxElementParent.Add("@name='" + elementParent.Attribute("name").Value + "'");
                    }

                    element = elementParent;
                    elementParent = elementParent.Parent;
                }


                aux.Clear();
                if (nodeIOS.Element.Attribute("label") != null && nodeIOS.Element.Attribute("label").Value != "")
                {
                    aux.Add("@label='" + CleanValueAttributeToSelector(nodeIOS.Element.Attribute("label").Value) + "'");
                }
                else if (nodeIOS.Element.Attribute("value") != null && nodeIOS.Element.Attribute("value").Value != "")
                {
                    aux.Add("@value='" + CleanValueAttributeToSelector(nodeIOS.Element.Attribute("value").Value) + "'");
                }
                else if (nodeIOS.Element.Attribute("name") != null && nodeIOS.Element.Attribute("name").Value != "")
                {
                    auxElementParent.Add("@name='" + nodeIOS.Element.Attribute("name").Value + "'");
                }

                if (auxElementParent.Count > 0 && aux.Count > 0)
                    xpath.XPathForIOS = "//*[" + string.Join(" and ", auxElementParent.ToArray()) + "]//*[" + string.Join(" and ", aux.ToArray()) + "]";

            }

            selectors.Add(xpath);

            #endregion


            return selectors;
        }

        private static string CleanValueAttributeToSelector(string attrValue)
        {

            //string[] aux = attrValue.Split('\n');

            //if (aux.Length > 0)
            //    return aux[0];
            //else

             return attrValue;

        }

        public static List<XPathSelector> OrderBySelectorInOrder(List<XPathSelector> selectors)
        {
            List<XPathSelector> newOrder = new List<XPathSelector>();

            newOrder.Add(selectors.Where(o => o.Type == XPathSelector.XPathType.CrossPlatform).Single());
            newOrder.Add(selectors.Where(o => o.Type == XPathSelector.XPathType.ElementType).Single());
            newOrder.Add(selectors.Where(o => o.Type == XPathSelector.XPathType.IdentifyAttributes).Single());
            newOrder.Add(selectors.Where(o => o.Type == XPathSelector.XPathType.AncestorAttributes).Single());
            newOrder.Add(selectors.Where(o => o.Type == XPathSelector.XPathType.AncestorIndex).Single());
            newOrder.Add(selectors.Where(o => o.Type == XPathSelector.XPathType.AbsolutePath).Single());

            return newOrder;
        }

        public static string ExtractDeviceId(DeviceConfig deviceConfig)
        {
            return deviceConfig.AppName + " - " +
                   deviceConfig.TestCaseName + " - " +
                   deviceConfig.PlatformName.ToUpper() + " " +
                   deviceConfig.PlatformVersion + " - " +
                   deviceConfig.DeviceName + " ";
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

            //rules: nodeCn == nodeCref ? (sempre verificar n é igual a ref).

            #region Equivalence

            nodeCref.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaCompareAttributesValues = false;
            nodeCn.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaCompareAttributesValues = false;

            //Verifica se algum valor nodeCref é contido em Cn. Se sim, são equivalantes.
            foreach (var attr in nodeCref.KeyAttributes)
            {
                if (nodeCn.KeyAttributes.ContainsValue(attr.Value))
                {
                    nodeCref.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaCompareAttributesValues = true;
                    nodeCn.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaCompareAttributesValues = true;
                    break;
                }
            }

            #endregion

            #region Compatibility
            nodeCref.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaElementNegativelyPositioning= false;
            nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaElementNegativelyPositioning = false;

            if (nodeCref.ElementPositionX < 0 || nodeCref.ElementPositionY < 0)
            {
                nodeCref.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaElementNegativelyPositioning = true;
            }

            if (nodeCn.ElementPositionX < 0 || nodeCn.ElementPositionY < 0)
            { 
                nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaElementNegativelyPositioning = true;
            }

            if (compareTextSimilarity)
                ExtractTextSimilarity(nodeCref, nodeCn);

            if (compareScreenshotSimilarity)
                ExtractScreenshotSimilarity(nodeCref, nodeCn);

            if (compareScreenshotOCRSimilarity)
                ExtractScreenshotOCRSimilarity(nodeCref, nodeCn);

            ExtractRuntimeFasterDifference(nodeCref, nodeCn);

            #endregion
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
            if (node.ExecutationState.Xml == null)
                return attrsValues;

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

            //if (nodeCref.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeFaster)
            //{
            //    double percent = ((d2 - d1) / d1) * 100;
            //    nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeDifferencePercentage = percent;
            //}
            //else
            //{
            //    double percent = ((d1 - d2) / d2) * 100;
            //    nodeCn.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeDifferencePercentage = percent;
            //}

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

            if (cref.AppName.Trim() != cn.AppName.Trim())
            {
                msg = "App Name isn't equal.";
                return false;
            }

            if (cref.AppActivity.Trim() != cn.AppActivity.Trim())
            {
                msg = "AppActivity Name isn't equal.";
                return false;
            }

            if (cref.TestCaseName.Trim() == "" || cn.TestCaseName.Trim() ==  "")
            {
                msg = "Test Case Name is required.";
                return false;
            }


            if (cref.TestCaseName.Trim() != cn.TestCaseName.Trim())
            {
                msg = "Test Case Name isn't equal.";
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
                                                    <b>*ENP:</b> {0}
                                                    <br>
                                                    <b>*TSHWVC:</b> {1}%
                                                    <br>
                                                    <b>*TST:</b> {2}% 
                                                    <br>
                                                    <b>*SS:</b> {3}% 
                                                    <br>
                                                    <b>*SOCRS:</b> {4}% 
                                                    <br>
                                                    <b>*Runtime Faster:</b> {5} 
                                                    <br>
                                                    <b>*Runtime Cref:</b> {6} 
                                                    <br>
                                                    <b>*Runtime Cn:</b> {7}
                                                    <br>    
                                                    <br>
                                                    <div class=""titleCriteria"">
                                                        EQUIVALENCE CRITERIA
                                                    </div>
                                                    <b>*CAV:</b> {8}
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

            for (i = 0; i < cn.Events.Count; i++)
            {
                MyNode crefNode = cref.Events[i];
                MyNode nNode = cn.Events[i];

                string textSimilarityHybridPercentage = "-";
                string textSimilarityTotalPercentage = "-";
                string guiScreenshotSimilarityPercentage = "-";
                string guiScreenshotOCRSimilarityPercentage = "-";
                string runtimeCref = "-";
                string runtimeCn = "-";
                string runtimeFaster = "-";

                string elementNegativelyPositioning = "-";

                string compareAttributesValues = "";

                if (compareTextSimilarity)
                {
                    textSimilarityHybridPercentage = Math.Round(nNode.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaTextSimilarityHybridPercentage, 2).ToString();
                    textSimilarityTotalPercentage = Math.Round(nNode.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaTextSimilarityTotalPercentage, 2).ToString();
                }

                if (compareScreenshotSimilarity)
                {
                    guiScreenshotSimilarityPercentage = Math.Round(nNode.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaGUIScreenshotSimilarityPercentage, 2).ToString();
                }

                if (compareScreenshotOCRSimilarity)
                {
                    guiScreenshotOCRSimilarityPercentage = Math.Round(nNode.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaGUIScreenshotOCRSimilarityPercentage, 2).ToString();
                }
                
                runtimeCref = Math.Round(crefNode.ExecutationState.RuntimePercent, 2).ToString() + "%";
                runtimeCn = Math.Round(nNode.ExecutationState.RuntimePercent, 2) + "%";

                if (crefNode.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaRuntimeFaster)
                    runtimeFaster = "Cref";
                else runtimeFaster = "Cn";

                elementNegativelyPositioning = nNode.ExecutationState.VerificationStateCriteria.CompatibilityCriteriaElementNegativelyPositioning.ToString();

                compareAttributesValues = nNode.ExecutationState.VerificationStateCriteria.EquivalenceCriteriaCompareAttributesValues.ToString();

                nodesCompare.Add(string.Format(templateComparisonNodes,
                    elementNegativelyPositioning,
                    textSimilarityHybridPercentage,
                    textSimilarityTotalPercentage,
                    guiScreenshotSimilarityPercentage,
                    guiScreenshotOCRSimilarityPercentage,
                    runtimeFaster,
                    runtimeCref,
                    runtimeCn,
                    compareAttributesValues)); 
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


        public MyNode Clone()
        {
            MyNode clone = new MyNode();

            clone.Platform = this.Platform;
            clone.Element = this.Element;
            clone.ElementType = this.ElementType;
            clone.XMLDoc = this.XMLDoc;
            clone.GUIScreenshot = this.GUIScreenshot;
            clone.ElementXPathAbsoluteSelector = this.ElementXPathAbsoluteSelector;
            clone.ElementType = this.ElementType;
            clone.KeyAttributes = this.KeyAttributes;


            return clone;
        }
    }

}
