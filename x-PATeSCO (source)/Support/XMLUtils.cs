using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrossPlatformCompatibility.Support
{
    public static class XMLUtils
    {
        public static string GetXPath(XElement element)
        {
            return string.Join("/", element.AncestorsAndSelf().Reverse()
                .Select(e =>
                {
                    var index = GetIndex(e);

                    if (index == 1)
                    {
                        return e.Name.LocalName;
                    }

                    return string.Format("{0}[{1}]", e.Name.LocalName, GetIndex(e));
                }));

        }

        public static int GetIndex(XElement element)
        {
            var i = 1;

            if (element.Parent == null)
            {
                return 1;
            }

            foreach (var e in element.Parent.Elements(element.Name.LocalName))
            {
                if (e == element)
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public static int GetIndexInParent(XElement element, XElement elementParent)
        {
            var i = 1;

            List<XElement> elements = elementParent.Elements().ToList();

            foreach (var e in elements)
            {
                if (e == element)
                {
                    break;
                }

                i++;
            }

            return i;
        }

        public static XElement GetImmediateParent(XElement element)
        {
            return element.Parent;
        }
    }
}
