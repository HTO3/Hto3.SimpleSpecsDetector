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
            var networkCards = Directory.EnumerateDirectories("/sys/devices/virtual/net", "eth*");

            foreach (var networkCard in networkCards)
            {
                yield return new NetworkCard()
                {
                    MACAddress = File.ReadAllText(Path.Combine(networkCard, "address")),
                    DeviceID = Path.GetDirectoryName(networkCard)                    
                };
            }
        }
    }
}
