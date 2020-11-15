using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class VideoDetector : IVideoDetector
    {        
        public String GetDisplayAdapterName()
        {
            return null;
        }

        public Int64? GetVideoMemory()
        {
            return 0;
        }

        public USize? GetCurrentVideoResolution()
        {
            return default(USize);
        }        
    }
}
