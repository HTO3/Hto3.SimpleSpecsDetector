using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    /// <summary>
    /// Represents a hard disk.
    /// </summary>
    public struct Disk
    {
        internal Disk(UInt64 size, String name, String deviceID)
        {
            this.Size = size;
            this.Name = name;
            this.DeviceID = deviceID;
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
        /// Unique identifier of the device.
        /// </summary>
        public String DeviceID { get; internal set; }
        /// <summary>
        /// Displays a sanitized representation of the disk.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{Name={this.Name}, Size={this.Size}}}";
        }
    }
}
