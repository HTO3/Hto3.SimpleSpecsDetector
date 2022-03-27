#if WINDOWS
using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class NetworkDetector : INetworkDetector
    {
        [DllImport("iphlpapi.dll")]
        private static extern uint GetIfEntry2(ref MibIfRow2 pIfRow);

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

        public async Task<NetworkThroughput> GetNetworkThroughput(String connectionName)
        {
            if (String.IsNullOrWhiteSpace(connectionName))
                throw new ArgumentException("The connection name is null or empty.", nameof(connectionName));

            var index = -1;

            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var netClasskey = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}", false))
            {
                var networkInterfaceKeyNames = netClasskey.GetSubKeyNames();

                foreach (var networkInterfaceKeyName in networkInterfaceKeyNames)
                {
                    var NetSetupPropertiesKey = netClasskey.OpenSubKey($@"{networkInterfaceKeyName}\NetSetupProperties");
                    if (NetSetupPropertiesKey == null)
                        continue;

                    var netAlias = (String)NetSetupPropertiesKey.GetValue("NETSETUPPKEY_Interface_IfAliasBase");

                    if (connectionName == netAlias)
                    {
                        index = Int32.Parse(networkInterfaceKeyName);
                        break;
                    }
                }

                if (index == -1)
                    throw new KeyNotFoundException("Network interface not found.");
            }
            
            var pIfRow1 = new MibIfRow2() { interfaceIndex = (UInt32)index };
            var pIfRow2 = new MibIfRow2() { interfaceIndex = (UInt32)index };

            GetIfEntry2(ref pIfRow1);
            await Task.Delay(1000);
            GetIfEntry2(ref pIfRow2);

            return new NetworkThroughput(connectionName, pIfRow2.inOctets - pIfRow1.inOctets, pIfRow2.outOctets - pIfRow1.outOctets);
        }
    }
}
#endif