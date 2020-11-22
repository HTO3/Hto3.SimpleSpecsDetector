using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal class MemoryDetector : IMemoryDetector
    {
        public UInt64 GetFreeMemory()
        {
            var meminfo = File.ReadAllText("/proc/meminfo");
            var memFreeMatch = Regex.Match(meminfo, @"MemFree:\s+(?<value>\d+)\skB");

            var memFreeValue = UInt64.Parse(memFreeMatch.Groups["value"].Value);
            return memFreeValue * 1000;
        }

        public UInt64 GetInstalledMemory()
        {
            var meminfo = File.ReadAllText("/proc/meminfo");
            var memtotalMatch = Regex.Match(meminfo, @"MemTotal:\s+(?<value>\d+)\skB");

            var memtotalValue = UInt64.Parse(memtotalMatch.Groups["value"].Value);
            return memtotalValue * 1000;
        }

        public UInt64 GetVisibleMemory()
        {
            var meminfo = File.ReadAllText("/proc/meminfo");
            var memAvailableMatch = Regex.Match(meminfo, @"MemAvailable:\s+(?<value>\d+)\skB");

            var memAvailableValue = UInt64.Parse(memAvailableMatch.Groups["value"].Value);
            return memAvailableValue * 1000;
        }
    }
}
