﻿using System;
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
            var wql = new ObjectQuery("SELECT Caption, Size FROM Win32_DiskDrive");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                foreach (var managementObject in searcher.Get())
                {
                    var name = (String)managementObject["Caption"];
                    var size = (UInt64)managementObject["Size"];

                    yield return new Disk(size, name);
                }                
            }
        }
        /// <summary>
        /// Represents a hard disk
        /// </summary>
        public struct Disk
        {
            internal Disk(UInt64 size, String name)
            {
                this.Size = size;
                this.Name = name;
            }
            /// <summary>
            /// Name/model of the HardDisk
            /// </summary>
            public String Name { get; internal set; }
            /// <summary>
            /// Size in bytes
            /// </summary>
            public UInt64 Size { get; internal set; }            
            /// <summary>
            /// Displays a sanitized representation of the disk 
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"{{Name={this.Name}, Size={this.Size}}}";
            }
        }
    }
}
