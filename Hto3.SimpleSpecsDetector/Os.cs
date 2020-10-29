using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    /// <summary>
    /// Information about the OS
    /// </summary>
    public static class Os
    {
        /// <summary>
        /// Get the correct Windows version following the Microsoft table (https://docs.microsoft.com/en-us/windows/win32/sysinfo/operating-system-version).
        /// </summary>
        /// <returns></returns>
        public static Decimal GetWindowsVersionNumber()
        {
            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
            {
                var versionCulture = new CultureInfo("en-US");

                if (subkey.GetValue("CurrentMajorVersionNumber") != null && subkey.GetValue("CurrentMinorVersionNumber") != null)
                {
                    var major = subkey.GetValue("CurrentMajorVersionNumber").ToString();
                    var minor = subkey.GetValue("CurrentMinorVersionNumber").ToString();

                    return Decimal.Parse($"{major}.{minor}", versionCulture);
                }

                return Decimal.Parse((String)subkey.GetValue("CurrentVersion"), versionCulture);
            }
        }

        /// <summary>
        /// Get the Windows version name.
        /// </summary>
        /// <returns></returns>
        public static String GetWindowsVersionName()
        {
            var wql = new ObjectQuery("SELECT Caption FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Caption"];
            }
        }
        /// <summary>
        /// Get the higher .NET Framework version installed on machine. This method can detect starting from 4.0.0 version.
        /// </summary>
        /// <returns></returns>
        public static String GetInstalledFrameworkVersion()
        {
            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", false) ?? key.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client", false))
            {
                var versionString = (String)subkey.GetValue("Version");
                var versionParts = versionString.Split('.');

                if (versionParts[0] == "4" && versionParts[1] == "0")
                    return "4.0.0";
                else
                {
                    var release = (Int32)subkey.GetValue("Release");

                    if (release >= 528040)
                        return "4.8.0";
                    if (release >= 461808)
                        return "4.7.2";
                    if (release >= 461308)
                        return "4.7.1";
                    if (release >= 460798)
                        return "4.7.0";
                    if (release >= 394802)
                        return "4.6.2";
                    if (release >= 394254)
                        return "4.6.1";
                    if (release >= 393295)
                        return "4.6.0";
                    if (release >= 379893)
                        return "4.5.2";
                    if (release >= 378675)
                        return "4.5.1";
                    if (release >= 378389)
                        return "4.5.0";
                }

                return null;
            }
        }
        /// <summary>
        /// Get system up time.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetSystemUpTime()
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount);
        }
    }
}
