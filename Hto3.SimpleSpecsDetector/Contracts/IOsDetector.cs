using System;
using System.Collections.Generic;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Contracts
{
    /// <summary>
    /// Information about the OS
    /// </summary>
    public interface IOsDetector : IDetector
    {
        /// <summary>
        /// Windows: Get the correct Windows version following the Microsoft table (https://docs.microsoft.com/en-us/windows/win32/sysinfo/operating-system-version).
        /// Linux:
        /// </summary>
        /// <returns></returns>
        Decimal GetOsVersionNumber();
        /// <summary>
        /// Get the OS version name.
        /// </summary>
        /// <returns></returns>
        String GetOsVersionName();
        /// <summary>
        /// Windows ONLY: Get the higher .NET Framework version installed on machine. This method can detect starting from 4.0.0 version.
        /// </summary>
        /// <returns></returns>
        String GetInstalledFrameworkVersion();
        /// <summary>
        /// Get system up time.
        /// </summary>
        /// <returns></returns>
        TimeSpan GetSystemUpTime();
    }
}
