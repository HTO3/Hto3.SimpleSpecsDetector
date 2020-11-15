using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    /// <summary>
    /// Represents a screen size.
    /// </summary>
    public struct USize
    {
        internal USize(UInt32 width, UInt32 height)
        {
            this.Width = width;
            this.Height = height;
        }
        /// <summary>
        /// Width of the screen in pixels
        /// </summary>
        public UInt32 Width { get; internal set; }
        /// <summary>
        /// Height of the screen in pixels.
        /// </summary>
        public UInt32 Height { get; internal set; }
        /// <summary>
        /// Displays a sanitized representation of the screen size. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{Width={this.Width}, Height={this.Height}}}";
        }
    }
}
