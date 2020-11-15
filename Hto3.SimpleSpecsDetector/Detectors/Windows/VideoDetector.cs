using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class VideoDetector : IVideoDetector
    {
        public String GetDisplayAdapterName()
        {
            var wql = new ObjectQuery("SELECT Name FROM Win32_VideoController");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                var managementObjectCollection = searcher.Get();
                if (managementObjectCollection.Count == 0)
                    return null;
                return (String)managementObjectCollection.Cast<ManagementObject>().First()["Name"];
            }
        }
        
        public Int64? GetVideoMemory()
        {
            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var subkey = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000", false))
            {
                var sizeFromDriver = (Int64?)subkey?.GetValue("HardwareInformation.qwMemorySize");

                //if it finds the amount of RAM reported by the driver
                if (sizeFromDriver.HasValue)
                    return sizeFromDriver.Value;
                //otherwise, picks up through Windows (not so trustable)
                else
                {
                    var wql = new ObjectQuery("SELECT AdapterRAM FROM Win32_VideoController");
                    using (var searcher = new ManagementObjectSearcher(wql))
                    {
                        var managementObjectCollection = searcher.Get();
                        if (managementObjectCollection.Count == 0)
                            return null;
                        return Convert.ToInt64(managementObjectCollection.Cast<ManagementObject>().First()["AdapterRAM"] ?? 0);
                    }
                }
            }
        }
        
        public USize? GetCurrentVideoResolution()
        {
            //first attempt
            var wql1 = new ObjectQuery("SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController");
            using (var searcher1 = new ManagementObjectSearcher(wql1))
            {
                var managementObjectCollection = searcher1.Get();
                if (managementObjectCollection.Count > 0)
                {
                    var managementObject = managementObjectCollection.Cast<ManagementObject>().First();

                    if (managementObject["CurrentHorizontalResolution"] != null && managementObject["CurrentVerticalResolution"] != null)
                    {
                        var width = (UInt32)managementObject["CurrentHorizontalResolution"];
                        var height = (UInt32)managementObject["CurrentVerticalResolution"];

                        return new USize(width, height);
                    }
                }            
            }

            //second attempt
            var wql2 = new ObjectQuery("SELECT HorizontalResolution, VerticalResolution FROM Win32_DisplayControllerConfiguration");
            using (var searcher2 = new ManagementObjectSearcher(wql2))
            {
                var managementObject = searcher2.Get().Cast<ManagementObject>().First();

                var width = (UInt32)managementObject["HorizontalResolution"];
                var height = (UInt32)managementObject["VerticalResolution"];

                return new USize(width, height);
            }
        }        
    }
}
