using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// Information about the motherboard
    /// </summary>
    public static class Motherboard
    {
        /// <summary>
        /// Get the vendor name.
        /// </summary>
        /// <returns></returns>
        public static String GetVendorName()
        {
            var wql = new ObjectQuery("SELECT Manufacturer FROM Win32_ComputerSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Manufacturer"];
            }
        }

        /// <summary>
        /// Get the motherboard model.
        /// </summary>
        /// <returns></returns>
        public static String GetModel()
        {
            var wql = new ObjectQuery("SELECT Model FROM Win32_ComputerSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Model"];
            }
        }

        /// <summary>
        /// Get the BIOS version.
        /// </summary>
        /// <returns></returns>
        public static String GetBIOSVersion()
        {
            var wql = new ObjectQuery("SELECT SMBIOSBIOSVersion FROM Win32_BIOS");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["SMBIOSBIOSVersion"];
            }
        }
    }
}
