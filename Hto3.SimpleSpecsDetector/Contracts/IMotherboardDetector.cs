using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the motherboard
    /// </summary>
    public interface IMotherboardDetector : IDetector
    {
        /// <summary>
        /// Get the vendor name.
        /// </summary>
        /// <returns></returns>
        String GetVendorName();
        /// <summary>
        /// Get the motherboard model.
        /// </summary>
        /// <returns></returns>
        String GetModel();
        /// <summary>
        /// Get the BIOS version.
        /// </summary>
        /// <returns></returns>
        String GetBIOSVersion();
    }
}
