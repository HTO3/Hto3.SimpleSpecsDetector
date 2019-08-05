using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
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
                return (String)searcher.Get().Cast<ManagementObject>().First()["Name"];
            }
        }
        /// <summary>
        /// Get amount of memory of the display adapter. Result in bytes. 
        /// </summary>
        /// <returns></returns>
        public static Int64 GetVideoMemory()
        {
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
            var subkey = key.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000", false);
            var sizeFromDriver = (Int64?)subkey.GetValue("HardwareInformation.qwMemorySize");
            
            //if it finds the amount of RAM reported by the driver
            if (sizeFromDriver.HasValue)
                return sizeFromDriver.Value;
            //otherwise, picks up through Windows (not so trustable)
            else
            {
                var wql = new ObjectQuery("SELECT AdapterRAM FROM Win32_VideoController");
                using (var searcher = new ManagementObjectSearcher(wql))
                {
                    return Convert.ToInt64(searcher.Get().Cast<ManagementObject>().First()["AdapterRAM"] ?? 0);
                }
            }
        }

        /// <summary>
        /// Get the resolution in pixels of the current display in use (focused).
        /// </summary>
        /// <returns></returns>
        public static USize GetCurrentVideoResolution()
        {
            var wql = new ObjectQuery("SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                var managementObject = searcher.Get().Cast<ManagementObject>().First();

                var width = (UInt32)managementObject["CurrentHorizontalResolution"];
                var height = (UInt32)managementObject["CurrentVerticalResolution"];

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
