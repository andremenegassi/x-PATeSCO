using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatformCompatibility.Support
{
    public class DeviceConfig
    {
        public string PlatformName { get; set; }
        public string PlatformVersion { get; set; }
        public string DeviceName { get; set; }
        public string AppPackage { get; set; }
        public string TakesScreenshot { get; set; }
        public string SessionOverride { get; set; }
        public string Server { get; set; }
        /// <summary>
        /// For iOS
        /// </summary>
        public string UUID { get; set; }
        public string AppName { get; set;  }
        public string AppPath { get; set; }
        public string TestCaseName { get; set; }
        public string AppActivity { get; set; } 

        public List<MyNode> Events { get; set; }

    }
}
