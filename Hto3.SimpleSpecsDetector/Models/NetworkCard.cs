using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    /// <summary>
    /// Represent a network card
    /// </summary>
    public struct NetworkCard
    {
        internal NetworkCard(Boolean enabled, String deviceID, String name, String manufacturer, String macAddress)
        {
            this.NetEnabled = enabled;
            this.DeviceID = deviceID;
            this.Name = name;
            this.Manufacturer = manufacturer;
            this.MACAddress = macAddress;
        }

        /// <summary>
        /// True if the network is enabled for this device.
        /// </summary>
        public Boolean NetEnabled { get; internal set; }
        /// <summary>
        /// Unique identifier of the device.
        /// </summary>
        public String DeviceID { get; internal set; }
        /// <summary>
        /// Network card name.
        /// </summary>
        public String Name { get; internal set; }
        /// <summary>
        /// Manufacturer of the network card.
        /// </summary>
        public String Manufacturer { get; internal set; }
        /// <summary>
        /// MAC Address
        /// </summary>
        public String MACAddress { get; internal set; }

        public override string ToString()
        {
            return $"{{Name={this.Name}, Manufacturer={this.Manufacturer}, MACAddress={this.MACAddress}}}";
        }
    }
}
