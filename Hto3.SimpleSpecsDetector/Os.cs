using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    public static class Os
    {
        /// <summary>
        /// Get the correct Windows version
        /// </summary>
        /// <returns></returns>
        public static Decimal GetWindowsVersionNumber()
        {
            var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
            var subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
            return Decimal.Parse((String)subkey.GetValue("CurrentVersion"), new CultureInfo("en-US"));
        }

        /// <summary>
        /// Get the Windows version name
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
