using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Models
{
    /// <summary>
    /// Network throughput stats.
    /// </summary>
    public struct NetworkThroughput
    {
        internal NetworkThroughput(String connectionName, UInt64 bytesReceived, UInt64 bytesSent)
        {
            this.BytesReceived = bytesReceived;
            this.BytesSent = bytesSent;
            this.ConnectionName = connectionName;
        }
        /// <summary>
        /// Bytes received amount.
        /// </summary>
        public UInt64 BytesReceived { get; internal set; }
        /// <summary>
        /// Bytes sent amount.
        /// </summary>
        public UInt64 BytesSent { get; internal set; }
        /// <summary>
        /// Network connection name that the OS expose to end users.
        /// </summary>
        public String ConnectionName { get; internal set; }
        /// <summary>
        /// Displays a representation of the network throughput.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{ConnectionName={this.ConnectionName}, BytesReceived={this.BytesReceived}, BytesSent={this.BytesSent}}}"; 
        }
    }
}
