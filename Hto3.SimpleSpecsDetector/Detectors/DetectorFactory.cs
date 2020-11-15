using Hto3.SimpleSpecsDetector.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors
{
    internal static class DetectorFactory
    {
        private static Type[] availableDetectorTypes;

        internal static T CreateDetector<T>() where T : IDetector
        {
            if (availableDetectorTypes == null)
                availableDetectorTypes = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsClass && typeof(IDetector).IsAssignableFrom(t))
                    .ToArray();

            var targetPlatformTypes = default(IEnumerable<Type>);

#if NETFRAMEWORK
            targetPlatformTypes = availableDetectorTypes.Where(dt => dt.Namespace.Contains("Windows"));            
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                targetPlatformTypes = availableDetectorTypes.Where(dt => dt.Namespace.Contains(nameof(OSPlatform.Windows)));
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                targetPlatformTypes = availableDetectorTypes.Where(dt => dt.Namespace.Contains(nameof(OSPlatform.Linux)));
            else
                throw new NotImplementedException($"This platform is not supported.");
#endif

            var detectorType = targetPlatformTypes
                .Single(t => typeof(T).IsAssignableFrom(t));

            return (T)Activator.CreateInstance(detectorType);
        }
    }
}
