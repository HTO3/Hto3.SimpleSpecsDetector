using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    public static class Storage
    {
        /// <summary>
        /// Get all connected hard disks.
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

        public struct Disk
        {
            internal Disk(UInt64 size, String name)
            {
                this.Size = size;
                this.Name = name;
            }

            public String Name { get; internal set; }
            public UInt64 Size { get; internal set; }            

            public override string ToString()
            {
                return $"{{Name={this.Name}, Size={this.Size}}}";
            }
        }
    }
}
