using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class ProcessorDetector : IProcessorDetector
    {        
        public String GetProcessorName()
        {
            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("lscpu");
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var match = Regex.Match(stdout.ToString(), @"Model name:\s+(?<value>.+)");

            return match.Groups["value"].Value;
        }
    }
}
