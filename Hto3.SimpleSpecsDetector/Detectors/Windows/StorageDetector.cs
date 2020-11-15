using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class StorageDetector : IStorageDetector
    {
        public IEnumerable<Disk> GetDisks()
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
