using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal class MotherboardDetector : IMotherboardDetector
    {
        public String GetVendorName()
        {
            const String PATH = "/sys/devices/virtual/dmi/id/board_vendor";
            if (!File.Exists(PATH))
                return null;

            var vendorName = File.ReadAllText(PATH);

            return vendorName.Trim();
        }

        public String GetModel()
        {
            const String PATH = "/sys/devices/virtual/dmi/id/board_name";
            if (!File.Exists(PATH))
                return null;

            var vendorName = File.ReadAllText(PATH);

            return vendorName.Trim();
        }

        public String GetBIOSVersion()
        {
            const String PATH = "/sys/devices/virtual/dmi/id/bios_version";
            if (!File.Exists(PATH))
                return null;

            var vendorName = File.ReadAllText(PATH);

            return vendorName.Trim();
        }
    }
}
