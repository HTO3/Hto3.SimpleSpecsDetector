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
            if (File.Exists("/usr/bin/lshw") || File.Exists("/usr/sbin/lshw"))
                return GetNetworkCardsBy_lshw();
            else if (File.Exists("/usr/bin/ip") || File.Exists("/usr/sbin/ip"))
                return GetNetworkCardsBy_ipaddr();
            else
                return Enumerable.Empty<NetworkCard>();
        }

        private IEnumerable<NetworkCard> GetNetworkCardsBy_lshw()
        {
            var lshwStdout = Utils.RunCommand("lshw", "-xml -class network");
            var xdocument = XDocument.Parse(lshwStdout);

            var networkCards = xdocument.Root.Descendants("node");

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

        private IEnumerable<NetworkCard> GetNetworkCardsBy_ipaddr()
        {
            var ipStdout = Utils.RunCommand("ip", "-json link");

#if NETSTANDARD

            var networkCards = Newtonsoft.Json.Linq.JArray.Parse(ipStdout);

            foreach (var networkCard in networkCards)
            {
                if (networkCard.Value<String>("link_type") != "ether")
                    continue;

                yield return new NetworkCard()
                {
                    MACAddress = networkCard.Value<String>("address"),
                    DeviceID = networkCard.Value<Int32>("ifindex").ToString(),
                    NetEnabled = networkCard.Value<String>("operstate") == "UP",
                    Name = networkCard.Value<String>("ifname")
                };
            }

#elif NET5_0

            var networkCards = System.Text.Json.JsonDocument.Parse(ipStdout)
                .RootElement
                .EnumerateArray();

            foreach (var networkCard in networkCards)
            {
                if (networkCard.GetProperty("link_type").GetString() != "ether")
                    continue;

                yield return new NetworkCard()
                {
                    MACAddress = networkCard.GetProperty("address").GetString(),
                    DeviceID = networkCard.GetProperty("ifindex").GetInt32().ToString(),
                    NetEnabled = networkCard.GetProperty("operstate").GetString() == "UP",
                    Name = networkCard.GetProperty("ifname").GetString()
                };
            }
#endif
            yield break;
        }
    }
}
