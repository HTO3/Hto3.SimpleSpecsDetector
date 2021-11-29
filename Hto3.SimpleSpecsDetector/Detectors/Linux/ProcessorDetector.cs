using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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

        public Task<Single> GetProcessorUsage()
        {
            return GetProcessorUsage(1000);
        }

        public Task<Single> GetProcessorUsage(Int32 measureByMiliseconds)
        {
            return GetProcessorUsage(measureByMiliseconds, default(CancellationToken));
        }

        public async Task<Single> GetProcessorUsage(Int32 measureByMiliseconds, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var statCpuValues = new Int64[2][];
 
            for (var i = 0; i < 2; i++)
            {
                var stat = default(String);
                using (var reader = new StreamReader("/proc/stat"))
                    stat = reader.ReadLine();

                statCpuValues[i] = stat.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .Select(s => Int64.Parse(s))
                    .ToArray();

                if (i == 0)
                    await Task.Delay(measureByMiliseconds, cancellationToken);
            }

            var diffIdle = statCpuValues[1][3] - statCpuValues[0][3];
            var diffLoad = statCpuValues[1].Sum() - statCpuValues[0].Sum();
            var load = 1 - diffIdle / (diffLoad * 1f);

            return load;
        }
    }
}
