#if WINDOWS
using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//Disable warning on OS specific classes
#pragma warning disable CA1416

namespace Hto3.SimpleSpecsDetector.Detectors.Windows
{
    internal class ProcessorDetector : IProcessorDetector
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetSystemTimes(out FileTime lpIdleTime, out FileTime lpKernelTime, out FileTime lpUserTime);

        public String GetProcessorName()
        {
            var wql = new ObjectQuery("SELECT Name FROM Win32_Processor");
            using (var searcher = new ManagementObjectSearcher(wql))
            {
                return (String)searcher.Get().Cast<ManagementObject>().First()["Name"];
            }
        }     

        public Task<Single> GetProcessorUsage()
        {
            return GetProcessorUsage(1000);
        }

        public Task<Single> GetProcessorUsage(Int32 measureByMiliseconds)
        {
            return GetProcessorUsage(measureByMiliseconds, default(CancellationToken));
        }

        public async Task<Single> GetProcessorUsage(Int32 measureByMiliseconds, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fileTimeToTimeSpan = new Func<FileTime, TimeSpan>(fileTime => TimeSpan.FromMilliseconds((((UInt64)fileTime.dwHighDateTime << 32) + (UInt32)fileTime.dwLowDateTime) * 0.000001));

            FileTime
                idleInitial,
                kernelInitial,
                userInitial,
                idleFinal,
                kernelFinal,
                userFinal;

            GetSystemTimes(out idleInitial, out kernelInitial, out userInitial);
            await Task.Delay(measureByMiliseconds, cancellationToken);
            GetSystemTimes(out idleFinal, out kernelFinal, out userFinal);

            TimeSpan idleInitialTs = fileTimeToTimeSpan(idleInitial);
            TimeSpan kernelInitialTs = fileTimeToTimeSpan(kernelInitial);
            TimeSpan userInitialTs = fileTimeToTimeSpan(userInitial);

            TimeSpan idleFinalTs = fileTimeToTimeSpan(idleFinal);
            TimeSpan kernelFinalTs = fileTimeToTimeSpan(kernelFinal);
            TimeSpan userFinalTs = fileTimeToTimeSpan(userFinal);

            TimeSpan idleDiffenceTs = idleFinalTs.Subtract(idleInitialTs);
            TimeSpan kernelDiffenceTs = kernelFinalTs.Subtract(kernelInitialTs);
            TimeSpan userDiffenceTs = userFinalTs.Subtract(userInitialTs);

            TimeSpan system = kernelDiffenceTs.Add(userDiffenceTs);

            var load = (Single)(system.Subtract(idleDiffenceTs).TotalMilliseconds / system.TotalMilliseconds);

            if (load > 1)
                load = 1;

            return load;
        }
    }
}
#endif