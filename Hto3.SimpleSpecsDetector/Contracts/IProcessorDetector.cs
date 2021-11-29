using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the Processor
    /// </summary>
    public interface IProcessorDetector : IDetector
    {
        /// <summary>
        /// Get the processor name.
        /// </summary>
        /// <returns></returns>
        String GetProcessorName();

        /// <summary>
        /// Mensure and get the current processor percent usage, where 0 is no load and 1 is full load. Default mensure time is 1 second.
        /// </summary>
        /// <param name="measureByMiliseconds">Measure CPU load during the informed time in miliseconds.</param>
        /// <param name="cancellationToken">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <returns></returns>
        Task<Single> GetProcessorUsage();

        /// <summary>
        /// Mensure and get the current processor percent usage, where 0 is no load and 1 is full load. Default mensure time is 1 second.
        /// </summary>
        /// <param name="measureByMiliseconds">Measure CPU load during the informed time in miliseconds.</param>
        /// <returns></returns>
        Task<Single> GetProcessorUsage(Int32 measureByMiliseconds);

        /// <summary>
        /// Mensure and get the current processor percent usage, where 0 is no load and 1 is full load. Default mensure time is 1 second.
        /// </summary>
        /// <param name="measureByMiliseconds">Measure CPU load during the informed time in miliseconds.</param>
        /// <param name="cancellationToken">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <returns></returns>
        Task<Single> GetProcessorUsage(Int32 measureByMiliseconds, CancellationToken cancellationToken);
    }
}
