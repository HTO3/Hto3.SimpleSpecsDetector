using Hto3.SimpleSpecsDetector.Contracts;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;

#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class OsDetector : IOsDetector
    {
        public String GetOsVersionNumber()
        {
            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default))
            using (var subkey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false))
            {
                if (subkey.GetValue("CurrentMajorVersionNumber") != null && subkey.GetValue("CurrentMinorVersionNumber") != null)
                {
                    var major = subkey.GetValue("CurrentMajorVersionNumber").ToString();
                    var minor = subkey.GetValue("CurrentMinorVersionNumber").ToString();

                    return $"{major}.{minor}";
                }

                return (String)subkey.GetValue("CurrentVersion");
            }
        }

        public String GetOsVersionName()
        {
            var wql = new ObjectQuery("SELECT Caption FROM Win32_OperatingSystem");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Caption"];
            }
        }

        public String GetInstalledFrameworkVersion()
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

        public TimeSpan GetSystemUpTime()
        {
            return TimeSpan.FromMilliseconds(Environment.TickCount);
        }

        public String GetKernelVersion()
        {
            var kernelVersionInfo = FileVersionInfo.GetVersionInfo(@"C:\Windows\System32\ntoskrnl.exe");
            return kernelVersionInfo.ProductVersion;
        }
    }
}
