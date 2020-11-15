using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class PrinterDetector : IPrinterDetector
    {
        public IEnumerable<PrinterDevice> GetPrinters()
        {
            return Enumerable.Empty<PrinterDevice>();
        }
    }
}
