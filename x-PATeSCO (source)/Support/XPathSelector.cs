using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatformCompatibility.Support
{
    public class XPathSelector
    {
        public enum XPathType
        {
            Indefined = 0,
            AbsolutePath = 1,
            IdentifyAttributes = 2,
            CrossPlatform = 3,
            ElementType = 4,
            AncestorIndex = 5,
            AncestorAttributes = 6
        }


        XPathType _type;
        string _xPathForAndroid;
        string _xPathForIOS;

        public XPathType Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
            }
        }

        public string XPathForAndroid
        {
            get
            {
                return _xPathForAndroid;
            }

            set
            {
                _xPathForAndroid = value;
            }
        }

        public string XPathForIOS
        {
            get
            {
                return _xPathForIOS;
            }

            set
            {
                _xPathForIOS = value;
            }
        }


        public XPathSelector()
        {
            _type = XPathType.Indefined;
            _xPathForAndroid = "";
            _xPathForIOS = "";

        }

    }


}
