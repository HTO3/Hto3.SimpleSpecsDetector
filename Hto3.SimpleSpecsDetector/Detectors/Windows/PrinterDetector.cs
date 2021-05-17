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
    internal class PrinterDetector : IPrinterDetector
    {
        public IEnumerable<PrinterDevice> GetPrinters()
        {
            var wql = new ObjectQuery("SELECT Caption, DriverName, Default FROM Win32_Printer");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                var enumerator = searcher.Get().GetEnumerator();

                while (true)
                {
                    var managementObject = default(ManagementBaseObject);

                    try
                    {
                        if (!enumerator.MoveNext())
                            break;

                        managementObject = enumerator.Current;
                    }
                    catch (ManagementException)
                    {
                        break;
                    }

                    var name = (String)managementObject["Caption"];
                    var driverName = (String)managementObject["DriverName"];
                    var defaultPrinter = (Boolean)managementObject["Default"];

                    yield return new PrinterDevice(name, driverName, defaultPrinter);
                }
            }
        }
    }
}
