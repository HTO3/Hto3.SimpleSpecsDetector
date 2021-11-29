using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    internal struct FileTime
    {
        /// <summary>
        /// Specifies the high 32 bits of the FILETIME.
        /// </summary>
        public int dwHighDateTime;
        /// <summary>
        /// Specifies the low 32 bits of the FILETIME.
        /// </summary>
        public int dwLowDateTime;
    }
}
