using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class ProcessorDetector : IProcessorDetector
    {        
        public String GetProcessorName()
        {
            var cpuinfo = File.ReadAllText("/proc/cpuinfo");
            var match = Regex.Match(cpuinfo, @"model\sname\s*:\s(?<value>.+)");

            return match.Groups["value"].Value;
        }
    }
}
