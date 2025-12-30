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

        [DllImport("iphlpapi.dll")]
        private static extern int GetIfTable2(out IntPtr Table);

        [DllImport("iphlpapi.dll")]
        private static extern void FreeMibTable(IntPtr Table);

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

            GetIfTable2(out var pTable);

            var table = Marshal.PtrToStructure<MibIfTable2>(pTable);
            var firstRead = default(MibIfRow2);

            for (int i = 0; i < table.NumEntries; i++)
            {
                var row = new MibIfRow2();
                row.interfaceIndex = (uint)i;
                GetIfEntry2(ref row);
                if (UShortArrayToString(row.description) == name)
                {
                    firstRead = row;
                    break;
                }
            }

            FreeMibTable(pTable);

            if (firstRead.Equals(default(MibIfRow2)))
                throw new KeyNotFoundException($"Network interface '{name}' not found.");

            cancellationToken.ThrowIfCancellationRequested();
            
            var secondRead = new MibIfRow2() { interfaceIndex = firstRead.interfaceIndex };

            await Task.Delay(1000, cancellationToken);

            GetIfEntry2(ref secondRead);

            return new NetworkThroughput(name, secondRead.inOctets - firstRead.inOctets, secondRead.outOctets - firstRead.outOctets);
        }

        private static string UShortArrayToString(char[] arr)
        {
            if (arr == null)
                return string.Empty;
            var length = Array.IndexOf(arr, '\0');
            if (length < 0)
                length = arr.Length;
            byte[] bytes = new byte[length * 2];
            Buffer.BlockCopy(arr, 0, bytes, 0, bytes.Length);
            return Encoding.Unicode.GetString(bytes);
        }
    }
}
#endif