using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the sound cards.
    /// </summary>
    public interface ISoundDetector : IDetector
    {
        /// <summary>
        /// Get all connected sound cards.
        /// </summary>
        /// <returns></returns>
        IEnumerable<SoundCard> GetSoundCards();
    }
}
