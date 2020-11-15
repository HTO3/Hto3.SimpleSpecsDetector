using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the network cards.
    /// </summary>
    public interface INetworkDetector : IDetector
    {
        /// <summary>
        /// Get all connected network cards.
        /// </summary>
        /// <returns></returns>
        IEnumerable<NetworkCard> GetNetworkCards();
    }
}
