using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the phisical memory
    /// </summary>
    public interface IMemoryDetector : IDetector
    {
        /// <summary>
        /// Number of bytes of physical memory currently unused and available.
        /// </summary>
        /// <returns></returns>
        UInt64 GetFreeMemory();
        /// <summary>
        /// Get the amount of installed physical memory in bytes.
        /// </summary>
        /// <returns></returns>
        UInt64 GetInstalledMemory();
        /// <summary>
        /// The total amount of physical memory (in Bytes) available to the OperatingSystem.
        /// This value does not necessarily indicate the true amount of physical memory, but
        /// what is reported to the OperatingSystem as available to it.
        /// </summary>
        /// <returns></returns>
        UInt64 GetVisibleMemory();
    }
}
