#if WINDOWS
using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class SoundDetector : ISoundDetector
    {
        public IEnumerable<SoundCard> GetSoundCards()
        {
            var wql = new ObjectQuery("SELECT Caption, Manufacturer, PNPDeviceID FROM Win32_SoundDevice");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                foreach (var managementObject in searcher.Get())
                {
                    var name = (String)managementObject["Caption"];
                    var manufacturer = (String)managementObject["Manufacturer"];
                    var deviceID = (String)managementObject["PNPDeviceID"];

                    yield return new SoundCard(name, manufacturer, deviceID);
                }
            }
        }
    }
}
#endif