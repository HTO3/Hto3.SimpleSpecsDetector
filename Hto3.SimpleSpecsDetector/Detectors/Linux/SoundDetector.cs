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
            if (!File.Exists("/usr/bin/lshw"))
                yield break;

            var lshwStdout = Utils.RunCommand("lshw", "-xml");
            var xdocument = XDocument.Parse(lshwStdout);

            var networkCards =
                xdocument.Root
                    .Element("node")
                        .Descendants("node")
                        .Where(n => n.Attribute("class").Value == "multimedia");

            foreach (var networkCard in networkCards)
            {
                yield return new SoundCard()
                {
                    DeviceID = networkCard.Element("physid")?.Value,
                    Name = networkCard.Element("product")?.Value,
                    Manufacturer = networkCard.Element("vendor")?.Value
                };
            }
        }
    }
}
