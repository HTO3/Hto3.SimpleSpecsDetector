using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal class OsDetector : IOsDetector
    {        
        public String GetOsVersionNumber()
        {
            var osrelease = File.ReadAllText("/etc/os-release");
            var osreleaseMatch = Regex.Match(osrelease, @"VERSION_ID=""(?<value>.+)""");

            return osreleaseMatch.Groups["value"].Value;
        }
        
        public String GetOsVersionName()
        {
            var osrelease = File.ReadAllText("/etc/os-release");
            var osreleaseMatch = Regex.Match(osrelease, @"PRETTY_NAME=""(?<value>.+)""");

            return osreleaseMatch.Groups["value"].Value;
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
            processStartInfo.RedirectStandardError = true;

            using (var statProcess = Process.Start(processStartInfo))
            {                
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var match = Regex.Match(stdout.ToString(), @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\.\d+\s(\+|-)\d{4}");
            
            return DateTime.Now - DateTime.Parse(match.Value);
        }

        public String GetKernelVersion()
        {
            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("uname", "-r");
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            return stdout.ToString().Trim();
        }
    }
}
