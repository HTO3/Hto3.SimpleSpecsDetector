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
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
            var subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
            var versionCulture = new CultureInfo("en-US");

            if (subkey.GetValue("CurrentMajorVersionNumber") != null && subkey.GetValue("CurrentMinorVersionNumber") != null)
            {
                var major = subkey.GetValue("CurrentMajorVersionNumber").ToString();
                var minor = subkey.GetValue("CurrentMinorVersionNumber").ToString();

                return Decimal.Parse($"{major}.{minor}", versionCulture);
            }

            return Decimal.Parse((String)subkey.GetValue("CurrentVersion"), versionCulture);
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
    }
}
