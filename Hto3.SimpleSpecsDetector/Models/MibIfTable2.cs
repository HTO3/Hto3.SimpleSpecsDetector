using System;
using System.Runtime.InteropServices;

namespace Hto3.SimpleSpecsDetector.Models
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MibIfTable2
    {
        public uint NumEntries;
        public IntPtr Table;
    }
}
