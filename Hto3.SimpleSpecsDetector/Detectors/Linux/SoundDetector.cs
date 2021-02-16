using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class SoundDetector : ISoundDetector
    {        
        public IEnumerable<SoundCard> GetSoundCards()
        {
            if (!File.Exists("/usr/bin/lshw") && !File.Exists("/usr/sbin/lshw"))
                yield break;

            var lshwStdout = Utils.RunCommand("lshw", "-xml -class multimedia");
            var xdocument = XDocument.Parse(lshwStdout);

            var soundCards = xdocument.Root.Descendants("node");

            foreach (var soundCard in soundCards)
            {
                yield return new SoundCard()
                {
                    DeviceID = soundCard.Element("physid")?.Value,
                    Name = soundCard.Element("product")?.Value,
                    Manufacturer = soundCard.Element("vendor")?.Value
                };
            }
        }
    }
}
