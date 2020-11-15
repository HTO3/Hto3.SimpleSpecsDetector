using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// A storage device on the computer
    /// </summary>
    public interface IStorageDetector : IDetector
    {
        /// <summary>
        /// Get all connected hard disks. The information available is the hard disk name and size (in Bytes).
        /// </summary>
        /// <returns></returns>
        IEnumerable<Disk> GetDisks();
    }
}
