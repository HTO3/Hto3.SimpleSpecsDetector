using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the display adapter.
    /// </summary>
    public interface IVideoDetector : IDetector
    {
        /// <summary>
        /// Get the display adapter name. Example NVIDIA GForce GTX 1080.
        /// </summary>
        /// <returns></returns>
        String GetDisplayAdapterName();
        /// <summary>
        /// Get amount of memory of the display adapter. Result in bytes. 
        /// </summary>
        /// <returns></returns>
        Int64? GetVideoMemory();
        /// <summary>
        /// Get the resolution in pixels of the current display in use (focused).
        /// </summary>
        /// <returns></returns>
        USize? GetCurrentVideoResolution();
    }
}
