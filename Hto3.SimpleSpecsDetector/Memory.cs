using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    public static class Memory
    {
        /// <summary>
        /// Get the available phisical memory in bytes.
        /// </summary>
        /// <returns></returns>
        public static UInt64 GetAvailableMemory()
        {
            var wql = new ObjectQuery("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return ((UInt64)searcher.Get().Cast<ManagementObject>().First<ManagementObject>()["FreePhysicalMemory"]) * 1024;
            }
        }

        /// <summary>
        /// Get the amount of installed phisical memory in bytes.
        /// </summary>
        /// <returns></returns>
        public static UInt64 GetInstalledMemory()
        {
            var wql = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (UInt64)searcher.Get().Cast<ManagementObject>().Select(m => Convert.ToInt64(m["Capacity"])).Sum();
            }
        }
    }
}
