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
    internal class NetworkDetector : INetworkDetector
    {        
        public IEnumerable<NetworkCard> GetNetworkCards()
        {
            if (Utils.IsInsideContainer)
                return GetNetworkCardsContainerMode();
            else
                return GetNetworkCardsPhysicalMode();
        }

        private IEnumerable<NetworkCard> GetNetworkCardsContainerMode()
        {
            var networkCards = Directory.EnumerateDirectories("/sys/devices/virtual/net", "eth*");

            foreach (var networkCard in networkCards)
            {
                yield return new NetworkCard()
                {
                    MACAddress = File.ReadAllText(Path.Combine(networkCard, "address")).Trim(),
                    DeviceID = File.ReadAllText(Path.Combine(networkCard, "dev_id")).Trim(),
                    NetEnabled = File.ReadAllText(Path.Combine(networkCard, "operstate")).Trim() == "up",
                    Name = Path.GetFileName(networkCard)
                };
            }
        }

        private IEnumerable<NetworkCard> GetNetworkCardsPhysicalMode()
        {
            if (!File.Exists("/usr/bin/lshw"))
                yield break;

            var lshwStdout = Utils.RunCommand("lshw", "-xml");
            var xdocument = XDocument.Parse(lshwStdout);

            var networkCards =
                xdocument.Root
                    .Element("node")
                        .Descendants("node")
                        .Where(n => n.Attribute("class").Value == "network");

            foreach (var networkCard in networkCards)
            {
                yield return new NetworkCard()
                {
                    MACAddress = networkCard.Element("serial")?.Value,
                    DeviceID = networkCard.Element("physid")?.Value,
                    NetEnabled = networkCard.Element("link")?.Value == "yes",
                    Name = networkCard.Element("logicalname")?.Value,
                    Manufacturer = networkCard.Element("vendor")?.Value
                };
            }
        }
    }
}
