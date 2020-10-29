using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// Information about the sound cards.
    /// </summary>
    public static class Sound
    {
        /// <summary>
        /// Get all connected sound cards.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SoundCard> GetSoundCards()
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
