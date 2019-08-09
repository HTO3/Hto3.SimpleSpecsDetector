using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// Information about the Processor
    /// </summary>
    public static class Processor
    {
        /// <summary>
        /// Get the processor name.
        /// </summary>
        /// <returns></returns>
        public static String GetProcessorName()
        {
            var wql = new ObjectQuery("SELECT Name FROM Win32_Processor");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Name"];
            }
        }
    }
}
