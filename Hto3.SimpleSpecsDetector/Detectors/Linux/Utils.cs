using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal static class Utils
    {
        private static Boolean? isInsideContainer;

        internal static Boolean IsInsideContainer
        {
            get
            {
                if (!isInsideContainer.HasValue)
                {
                    var cgroup = File.ReadAllText("/proc/1/cgroup");
                    isInsideContainer = cgroup.Contains("/docker/") || cgroup.Contains("/lxc/");
                }

                return isInsideContainer.Value;
            }
        }

        internal static String RunCommand(String command, String args = null)
        {
            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo(command, args);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            return stdout.ToString();
        }
    }
}
