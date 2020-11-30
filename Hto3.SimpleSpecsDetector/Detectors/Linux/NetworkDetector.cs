using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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
            return Enumerable.Empty<NetworkCard>();
        }
    }
}
