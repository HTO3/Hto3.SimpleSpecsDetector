using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// Information about the installed printers.
    /// </summary>
    public static class Printer
    {
        /// <summary>
        /// Get all installed printers.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PrinterDevice> GetPrinters()
        {
            var wql = new ObjectQuery("SELECT Caption, DriverName, Default FROM Win32_Printer");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                foreach (var managementObject in searcher.Get())
                {
                    var name = (String)managementObject["Caption"];
                    var driverName = (String)managementObject["DriverName"];
                    var defaultPrinter = (Boolean)managementObject["Default"];

                    yield return new PrinterDevice(name, driverName, defaultPrinter);
                }
            }
        }
    }
}
