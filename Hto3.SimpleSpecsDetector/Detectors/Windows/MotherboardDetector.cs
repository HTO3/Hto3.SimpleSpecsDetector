using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{    
    internal class MotherboardDetector : IMotherboardDetector
    {        
        public String GetVendorName()
        {
            var wql = new ObjectQuery("SELECT Manufacturer FROM Win32_ComputerSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Manufacturer"];
            }
        }
        
        public String GetModel()
        {
            var wql = new ObjectQuery("SELECT Model FROM Win32_ComputerSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Model"];
            }
        }
        
        public String GetBIOSVersion()
        {
            var wql = new ObjectQuery("SELECT SMBIOSBIOSVersion FROM Win32_BIOS");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["SMBIOSBIOSVersion"];
            }
        }
    }
}
