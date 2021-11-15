#if WINDOWS
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
    internal class ProcessorDetector : IProcessorDetector
    {
        public String GetProcessorName()
        {
            var wql = new ObjectQuery("SELECT Name FROM Win32_Processor");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Name"];
            }
        }
    }
}
#endif