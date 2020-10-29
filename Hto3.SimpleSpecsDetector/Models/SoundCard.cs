using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    /// <summary>
    /// Represents a sound card
    /// </summary>
    public struct SoundCard
    {
        internal SoundCard(String name, String manufacturer, String deviceID)
        {
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.DeviceID = deviceID;
        }
        /// <summary>
        /// Sound card name.
        /// </summary>
        public String Name { get; internal set; }
        /// <summary>
        /// Manufacturer of the sound device.
        /// </summary>
        public String Manufacturer { get; internal set; }
        /// <summary>
        /// Unique identifier of the device.
        /// </summary>
        public String DeviceID { get; internal set; }

        public override string ToString()
        {
            return $"{{Name={this.Name}, Manufacturer={this.Manufacturer}}}";
        }
    }
}
