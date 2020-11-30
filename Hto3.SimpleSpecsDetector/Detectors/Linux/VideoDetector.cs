﻿using Hto3.SimpleSpecsDetector.Contracts;
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

            var lspciProcessStartInfo = new ProcessStartInfo("lspci");
            lspciProcessStartInfo.RedirectStandardOutput = true;

            using (var statProcess = Process.Start(lspciProcessStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }
            
            var pciMatch = Regex.Match(stdout.ToString(), @"(?<value>\d{2}:\d{2}\.\d)\sVGA compatible controller:");

            if (!pciMatch.Success)
                return null;

            stdout.Clear();

            lspciProcessStartInfo = new ProcessStartInfo("lspci", $"-v -s {pciMatch.Groups["value"].Value}");
            lspciProcessStartInfo.RedirectStandardOutput = true;            

            using (var statProcess = Process.Start(lspciProcessStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var memoryMatches = Regex.Matches(stdout.ToString(), @"Memory\sat\s[^\[]+\[size=(?<size>\d+)(?<unit>\w)\]");

            if (memoryMatches.Count == 0)
                return null;

            var memoryMatch =
                memoryMatches
                    .Cast<Match>()
                    .FirstOrDefault(mm => mm.Value.Contains(" prefetchable"))
                ??
                memoryMatches
                    .Cast<Match>()
                    .FirstOrDefault(mm => mm.Value.Contains(" non-prefetchable"));

            if (memoryMatch == null)
                return null;

            var unit = memoryMatch.Groups["unit"].Value;
            var size = Int32.Parse(memoryMatch.Groups["size"].Value);

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
