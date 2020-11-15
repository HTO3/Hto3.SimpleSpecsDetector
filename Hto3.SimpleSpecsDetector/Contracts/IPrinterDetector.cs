using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the installed printers.
    /// </summary>
    public interface IPrinterDetector : IDetector
    {
        /// <summary>
        /// Get all installed printers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PrinterDevice> GetPrinters();
    }
}
