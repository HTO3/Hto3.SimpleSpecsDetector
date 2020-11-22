using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{
    internal static class Utils
    {
        private static Boolean? isInsideContainer;

        public static Boolean IsInsideContainer
        {
            get
            {
                if (!isInsideContainer.HasValue)
                {
                    var cgroup = File.ReadAllText("/proc/1/cgroup");
                    isInsideContainer = cgroup.Contains("/docker/") || cgroup.Contains("/lxc/");
                }

                return isInsideContainer.Value;
            }
        }

    }
}
