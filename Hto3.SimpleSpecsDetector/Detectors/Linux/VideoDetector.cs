using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class VideoDetector : IVideoDetector
    {        
        public String GetDisplayAdapterName()
        {
            if (!File.Exists("/usr/bin/lspci"))
                return null;

            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("lspci");
            processStartInfo.RedirectStandardOutput = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var match = Regex.Match(stdout.ToString(), @"VGA compatible controller:\s(?<value>.*)");

            if (!match.Success)
                return null;

            var vgaCardName = match.Groups["value"].Value;

            return vgaCardName;
        }

        public Int64? GetVideoMemory()
        {
            if (!File.Exists("/usr/bin/lspci"))
                return null;

            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("lspci");
            processStartInfo.RedirectStandardOutput = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var match = Regex.Match(stdout.ToString(), @"(?<value>\d{2}:\d{2}\.\d)\sVGA compatible controller:");

            if (!match.Success)
                return null;

            stdout.Clear();

            processStartInfo = new ProcessStartInfo("lspci", $"-v -s {match.Groups["value"].Value}");
            processStartInfo.RedirectStandardOutput = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            match = Regex.Match(stdout.ToString(), @"Memory\sat\s[^\[]+\[size=(?<size>\d+)(?<unit>\w)\]");

            if (!match.Success)
                return null;

            var unit = match.Groups["unit"].Value;
            var size = Int32.Parse(match.Groups["size"].Value);

            switch (unit)
            {
                case "K":
                    return size * 1000;
                case "M":
                    return size * 1000000;
                case "G":
                    return size * 1000000000;                
                default:
                    return null;
            }
        }

        public USize? GetCurrentVideoResolution()
        {
            if (!File.Exists("/usr/bin/xrandr"))
                return null;

            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("xrandr", "--current");
            processStartInfo.RedirectStandardOutput = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var match = Regex.Match(stdout.ToString(), @"connected\s\w+\s(?<width>\d+)x(?<height>\d+)");

            if (!match.Success)
                return null;

            var width = UInt32.Parse(match.Groups["width"].Value);
            var height = UInt32.Parse(match.Groups["height"].Value);
            var size = new USize(width, height);

            return size;
        }        
    }
}
