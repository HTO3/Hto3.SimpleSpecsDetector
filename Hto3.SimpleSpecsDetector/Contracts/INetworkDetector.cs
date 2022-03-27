using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        /// <summary>
        /// Mensure 1 second and get the current network traffic throughput. Result in Bytes.
        /// </summary>
        /// <param name="connectionName">Network connection name that the OS expose to end users.</param>
        /// <returns></returns>
        Task<NetworkThroughput> GetNetworkThroughput(String connectionName);
        /// <summary>
        /// Mensure 1 second and get the current network traffic throughput. Result in Bytes.
        /// </summary>
        /// <param name="name">Network name that the OS expose to end users.</param>
        /// <param name="cancellationToken">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <returns></returns>
        Task<NetworkThroughput> GetNetworkThroughput(String name, CancellationToken cancellationToken);    
    }
}
