﻿using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// A storage device on the computer
    /// </summary>
    public static class Storage
    {
        /// <summary>
        /// Get all connected hard disks. The information available is the hard disk name and size (in Bytes).
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Disk> GetDisks()
        {
            var wql = new ObjectQuery("SELECT Caption, Size, PNPDeviceID FROM Win32_DiskDrive");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                foreach (var managementObject in searcher.Get())
                {
                    var name = (String)managementObject["Caption"];
                    var size = (UInt64)managementObject["Size"];
                    var deviceID = (String)managementObject["PNPDeviceID"];

                    yield return new Disk(size, name, deviceID);
                }                
            }
        }        
    }
}
