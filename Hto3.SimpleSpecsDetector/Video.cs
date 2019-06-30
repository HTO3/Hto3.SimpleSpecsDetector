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
                return (String)searcher.Get().Cast<ManagementObject>().First<ManagementObject>()["Name"];
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
                    return Convert.ToInt64(searcher.Get().Cast<ManagementObject>().First<ManagementObject>()["AdapterRAM"] ?? 0);
                }
            }
        }
    }
}
