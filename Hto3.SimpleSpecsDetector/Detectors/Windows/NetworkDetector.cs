#if WINDOWS
using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class NetworkDetector : INetworkDetector
    {
        public IEnumerable<NetworkCard> GetNetworkCards()
        {
            var wqlText = "SELECT Name, PNPDeviceID, NetEnabled, Manufacturer, MACAddress FROM Win32_NetworkAdapter WHERE ";

            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkCards", false))
            {
                var physicalNetworkCardsEntries = subkey.GetSubKeyNames();

                for (var i = 0; i < physicalNetworkCardsEntries.Length; i++)
                {
                    using (var networkCardkey = subkey.OpenSubKey(physicalNetworkCardsEntries[i]))
                        wqlText += $"GUID = '{(String)networkCardkey.GetValue("ServiceName")}'";

                    if (i + 1 < physicalNetworkCardsEntries.Length)
                        wqlText += " OR ";
                }
            }
              
            var wql = new ObjectQuery(wqlText);
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                foreach (var managementObject in searcher.Get())
                {
                    var name = (String)managementObject["Name"];
                    var deviceID = (String)managementObject["PNPDeviceID"];
                    var netEnabled = (Boolean)managementObject["NetEnabled"];
                    var manufacturer = (String)managementObject["Manufacturer"];
                    var macAddress = (String)managementObject["MACAddress"];

                    yield return new NetworkCard(netEnabled, deviceID, name, manufacturer, macAddress);
                }
            }
        }
    }
}
#endif