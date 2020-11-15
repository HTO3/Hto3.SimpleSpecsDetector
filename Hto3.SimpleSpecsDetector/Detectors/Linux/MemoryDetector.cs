using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal class MemoryDetector : IMemoryDetector
    {
        public UInt64 GetFreeMemory()
        {
            return 0;
        }

        public UInt64 GetInstalledMemory()
        {
            return 0;
        }

        public UInt64 GetVisibleMemory()
        {
            return 0;
        }
    }
}
