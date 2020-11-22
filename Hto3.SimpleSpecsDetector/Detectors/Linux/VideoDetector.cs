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
            return null;
        }

        public Int64? GetVideoMemory()
        {
            return 0;
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

            var width = UInt32.Parse(match.Groups["width"].Value);
            var height = UInt32.Parse(match.Groups["height"].Value);
            var size = new USize(width, height);

            return size;
        }        
    }
}
