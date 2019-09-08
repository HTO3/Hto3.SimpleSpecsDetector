using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// Information about the display adapter.
    /// </summary>
    public class Video
    {
        /// <summary>
        /// Get the display adapter name. Example NVIDIA GForce GTX 1080.
        /// </summary>
        /// <returns></returns>
        public static String GetDisplayAdapterName()
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
        /// <summary>
        /// Get amount of memory of the display adapter. Result in bytes. 
        /// </summary>
        /// <returns></returns>
        public static Int64? GetVideoMemory()
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

        /// <summary>
        /// Get the resolution in pixels of the current display in use (focused).
        /// </summary>
        /// <returns></returns>
        public static USize GetCurrentVideoResolution()
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
        /// <summary>
        /// Represents a screen size.
        /// </summary>
        public struct USize
        {
            internal USize(UInt32 width, UInt32 height)
            {
                this.Width = width;
                this.Height = height;
            }

            public UInt32 Width { get; internal set; }
            public UInt32 Height { get; internal set; }

            public override string ToString()
            {
                return $"{{Width={this.Width}, Height={this.Height}}}";
            }
        }
    }
}
