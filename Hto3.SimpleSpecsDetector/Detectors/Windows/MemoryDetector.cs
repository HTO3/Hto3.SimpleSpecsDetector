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
    internal class MemoryDetector : IMemoryDetector
    {        
        public UInt64 GetFreeMemory()
        {
            var wql = new ObjectQuery("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return ((UInt64)searcher.Get().Cast<ManagementObject>().First()["FreePhysicalMemory"]) * 1000;
            }
        }
        
        public UInt64 GetInstalledMemory()
        {
            var wql = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (UInt64)searcher.Get().Cast<ManagementObject>().Select(m => Convert.ToInt64(m["Capacity"])).Sum();
            }
        }
        
        public UInt64 GetVisibleMemory()
        {
            var wql = new ObjectQuery("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return ((UInt64)searcher.Get().Cast<ManagementObject>().First()["TotalVisibleMemorySize"]) * 1000;
            }
        }
    }
}
