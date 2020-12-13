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
            if (!File.Exists("/usr/bin/lspci") && !File.Exists("/sbin/lspci"))
                return null;

            var lspciStdout = String.Empty;

            try { lspciStdout = Utils.RunCommand("lspci"); }
            catch { /*Some distos require sudo elavation to run lspci, at this moment, we'll not handle it.*/ }
 
            var match = Regex.Match(lspciStdout, @"VGA compatible controller:\s(?<value>.+)");

            if (!match.Success)
                return null;

            var vgaCardName = match.Groups["value"].Value;

            return vgaCardName;
        }

        public Int64? GetVideoMemory()
        {
            if (!File.Exists("/usr/bin/lspci") && !File.Exists("/sbin/lspci"))
                return null;

            var lspciStdout = String.Empty;

            try { lspciStdout = Utils.RunCommand("lspci"); }
            catch { /*Some distos require sudo elavation to run lspci, at this moment, we'll not handle it.*/ }

            var pciMatch = Regex.Match(lspciStdout, @"(?<value>\d{2}:\d{2}\.\d)\sVGA compatible controller:");

            if (!pciMatch.Success)
                return null;

            lspciStdout = Utils.RunCommand("lspci", $"-v -s {pciMatch.Groups["value"].Value}");
            var memoryMatches = Regex.Matches(lspciStdout, @"Memory\sat\s[^\[]+\[size=(?<size>\d+)(?<unit>\w)\]");

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

            var xrandrStdout = Utils.RunCommand("xrandr", "--current");
            var match = Regex.Match(xrandrStdout, @"connected\s\w+\s(?<width>\d+)x(?<height>\d+)");

            if (!match.Success)
                return null;

            var width = UInt32.Parse(match.Groups["width"].Value);
            var height = UInt32.Parse(match.Groups["height"].Value);
            var size = new USize(width, height);

            return size;
        }        
    }
}
