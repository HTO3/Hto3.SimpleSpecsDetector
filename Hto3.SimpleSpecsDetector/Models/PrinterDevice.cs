using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    /// <summary>
    /// Represent a printer.
    /// </summary>
    public struct PrinterDevice
    {
        internal PrinterDevice(String name, String driverName, Boolean defaultPrinter)
        {
            this.Name = name;
            this.DriverName = driverName;
            this.DefaultPrinter = defaultPrinter;
        }

        /// <summary>
        /// Printer name.
        /// </summary>
        public String Name { get; internal set; }
        /// <summary>
        /// Printer driver name.
        /// </summary>
        public String DriverName { get; internal set; }
        /// <summary>
        /// True if this is the default printer.
        /// </summary>
        public Boolean DefaultPrinter { get; internal set; }
        /// <summary>
        /// Displays a sanitized representation of the printer. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{Name={this.Name}, DefaultPrinter={this.DefaultPrinter}}}";
        }
    }
}
