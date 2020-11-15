using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class StorageDetector : IStorageDetector
    {        
        public IEnumerable<Disk> GetDisks()
        {
            return Enumerable.Empty<Disk>();
        }        
    }
}
