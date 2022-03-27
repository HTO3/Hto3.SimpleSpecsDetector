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
using System.Threading;
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

        public Task<NetworkThroughput> GetNetworkThroughput(String connectionName)
        {
            return GetNetworkThroughput(connectionName, default(CancellationToken));
        }

        public async Task<NetworkThroughput> GetNetworkThroughput(String name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("The network name is null or empty.", nameof(name));

            var index = -1;

            using (var localMachinekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var netClassKey = localMachinekey.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}", false))
            {
                var networkInterfaceKeyNames = netClassKey.GetSubKeyNames();

                foreach (var networkInterfaceKeyName in networkInterfaceKeyNames)
                {
                    if (!Int16.TryParse(networkInterfaceKeyName, out Int16 _))
                        continue;

                    using (var deviceInstanceIDKey = netClassKey.OpenSubKey(networkInterfaceKeyName))
                    {
                        var deviceInstanceID = (String)deviceInstanceIDKey.GetValue("DeviceInstanceID");

                        using (var deviceKey = localMachinekey.OpenSubKey($@"SYSTEM\CurrentControlSet\Enum\{deviceInstanceID}"))
                        {
                            var friendlyName = (String)deviceKey.GetValue("FriendlyName");

                            if (name == friendlyName)
                            {
                                index = Int32.Parse(networkInterfaceKeyName);
                                break;
                            }
                        }
                    }
                }

                if (index == -1)
                    throw new KeyNotFoundException("Network interface not found.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var pIfRow1 = new MibIfRow2() { interfaceIndex = (UInt32)index };
            var pIfRow2 = new MibIfRow2() { interfaceIndex = (UInt32)index };

            GetIfEntry2(ref pIfRow1);
            await Task.Delay(1000, cancellationToken);
            GetIfEntry2(ref pIfRow2);

            return new NetworkThroughput(name, pIfRow2.inOctets - pIfRow1.inOctets, pIfRow2.outOctets - pIfRow1.outOctets);
        }
    }
}
#endif