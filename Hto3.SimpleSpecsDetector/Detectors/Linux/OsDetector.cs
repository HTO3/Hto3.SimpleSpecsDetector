using Hto3.SimpleSpecsDetector.Contracts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal class OsDetector : IOsDetector
    {        
        public Decimal GetOsVersionNumber()
        {
            return 0;
        }
        
        public String GetOsVersionName()
        {
            return null;
        }
        
        public String GetInstalledFrameworkVersion()
        {
            //.NET Framework don't exists on Linux environment.
            return null;
        }
        
        public TimeSpan GetSystemUpTime()
        {
            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("stat", "/proc/1/");
            processStartInfo.RedirectStandardOutput = true;

            using (var statProcess = Process.Start(processStartInfo))
            {                
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var match = Regex.Match(stdout.ToString(), @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\.\d+\s\+\d{4}");
            
            return DateTime.Now - DateTime.Parse(match.Value);
        }
    }
}
