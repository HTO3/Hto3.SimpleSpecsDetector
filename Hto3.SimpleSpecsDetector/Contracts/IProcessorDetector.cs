using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the Processor
    /// </summary>
    public interface IProcessorDetector : IDetector
    {
        /// <summary>
        /// Get the processor name.
        /// </summary>
        /// <returns></returns>
        String GetProcessorName();
    }
}
