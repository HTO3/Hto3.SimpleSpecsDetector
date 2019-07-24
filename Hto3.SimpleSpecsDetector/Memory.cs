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
        /// Number of bytes of physical memory currently unused and available.
        /// </summary>
        /// <returns></returns>
        public static UInt64 GetFreeMemory()
        {
            var wql = new ObjectQuery("SELECT FreePhysicalMemory FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return ((UInt64)searcher.Get().Cast<ManagementObject>().First()["FreePhysicalMemory"]) * 1000;
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

        /// <summary>
        /// The total amount of physical memory (in Kbytes) available to the OperatingSystem.
        /// This value does not necessarily indicate the true amount of physical memory, but
        /// what is reported to the OperatingSystem as available to it.
        /// </summary>
        /// <returns></returns>
        public static UInt64 GetVisibleMemory()
        {
            var wql = new ObjectQuery("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return ((UInt64)searcher.Get().Cast<ManagementObject>().First()["TotalVisibleMemorySize"]) * 1000;
            }
        }
    }
}
