using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                var serial = networkCard.Element("serial")?.Value;
                var vendor = networkCard.Element("vendor")?.Value;
                var physid = networkCard.Element("physid")?.Value;
                var link = networkCard.Element("link")?.Value;
                var logicalname = networkCard.Element("logicalname")?.Value;

                yield return new NetworkCard()
                {
                    MACAddress = serial,
                    DeviceID = physid,
                    NetEnabled = link == "yes",
                    Name = logicalname,
                    Manufacturer = String.IsNullOrEmpty(vendor) && !String.IsNullOrEmpty(serial) ? GetNetworkInterfaceVendorNameByMACAddress(serial) : vendor
                };
            }
        }

        private IEnumerable<NetworkCard> GetNetworkCardsBy_ipaddr()
        {
            var ipStdout = Utils.RunCommand("ip", "link");

            var lines = ipStdout.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i += 2)
            {
                if (!lines[i + 1].Contains("link/ether"))
                    continue;

                var firstLineMatch = Regex.Match(lines[i], @"(?<id>\d+):\s(?<name>.+):\s<(?<tags>.+)>\s(?<key_value_pairs>.+)");
                var id = firstLineMatch.Groups["id"].Value;
                var name = firstLineMatch.Groups["name"].Value;

                var rawKeyValuePairs = firstLineMatch.Groups["key_value_pairs"].Value
                    .Split(' ')
                    .Union
                    (
                        lines[i + 1]
                            .Trim()
                            .Split(' ')
                    )
                    .ToArray();

                var properties = new Dictionary<String, String>();
                for (var j = 0; j < rawKeyValuePairs.Length; j += 2)
                    properties.Add(rawKeyValuePairs[j], rawKeyValuePairs[j + 1]);

                var manufacturer = GetNetworkInterfaceVendorNameByMACAddress(properties["link/ether"]);

                yield return new NetworkCard()
                {
                    MACAddress = properties["link/ether"],
                    DeviceID = id,
                    NetEnabled = properties["state"] == "UP",
                    Name = name,
                    Manufacturer = manufacturer
                };
            }
        }

        private String GetNetworkInterfaceVendorNameByMACAddress(String macAddress)
        {
            var line = default(String);
            //http://standards-oui.ieee.org/oui/oui.txt
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hto3.SimpleSpecsDetector.Resources.oui.txt");
            using (var streamReader = new StreamReader(resourceStream, Encoding.UTF8, false, 1024))
            {
                var oui = macAddress
                    .Substring(0, 8)
                    .ToUpper()
                    .Replace(':', '-');

                do
                {
                    line = streamReader.ReadLine();
                }
                while
                (
                    line != null
                    &&
                    !line.StartsWith(oui)
                );
            }

            return line?.Substring(line.IndexOf("\t\t") + 2);
        }

        public async Task<NetworkThroughput> GetNetworkThroughput(String connectionName)
        {
            if (String.IsNullOrWhiteSpace(connectionName))
                throw new ArgumentException("The network interface name is null or empty.", nameof(connectionName));

            if (!Directory.Exists($"/sys/class/net/{connectionName}"))
                throw new KeyNotFoundException("Network interface not found.");

            var receivedBytes1 = UInt64.Parse(File.ReadAllText($"/sys/class/net/{connectionName}/statistics/rx_bytes"));
            var sentBytes1 = UInt64.Parse(File.ReadAllText($"/sys/class/net/{connectionName}/statistics/tx_bytes"));
            await Task.Delay(1000);
            var receivedBytes2 = UInt64.Parse(File.ReadAllText($"/sys/class/net/{connectionName}/statistics/rx_bytes"));
            var sentBytes2 = UInt64.Parse(File.ReadAllText($"/sys/class/net/{connectionName}/statistics/tx_bytes"));

            return new NetworkThroughput(connectionName, receivedBytes2 - receivedBytes1, sentBytes2 - sentBytes1);
        }
    }
}
