using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class StorageDetector : IStorageDetector
    {        
        public IEnumerable<Disk> GetDisks()
        {
            if (File.Exists("/usr/bin/lsblk") || File.Exists("/usr/sbin/lsblk"))
                return GetDisksBylsblk();
            else
                return GetDisksByProcPartitions();
        }

        private IEnumerable<Disk> GetDisksByProcPartitions()
        {
            var lines = File.ReadAllLines("/proc/partitions");

            for (var i = 2; i < lines.Length; i++)
            {
                var parts = lines[i].Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (!parts[3].StartsWith("sd"))
                    continue;

                var devicePath = Utils.RunCommand("readlink", $"-f /sys/class/block/{parts[3]}");
                var match = Regex.Match(devicePath, "[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}");

                yield return new Disk()
                {
                    DeviceID = match.Value,
                    Name = parts[3],
                    Size = UInt64.Parse(parts[2]) * 1000
                };
            }
        }

        private IEnumerable<Disk> GetDisksBylsblk()
        {
            const String COLUMN_WWN = "WWN";
            const String COLUMN_NAME = "NAME";
            const String COLUMN_SIZE = "SIZE";
            const String COLUMN_TYPE = "TYPE";

            var lsblkStdout = Utils.RunCommand("lsblk", $"-d -b -P -o {COLUMN_WWN},{COLUMN_NAME},{COLUMN_SIZE},{COLUMN_TYPE}");

            var disks =
                lsblkStdout
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(l => l.Contains("TYPE=\"disk\""));

            foreach (var disk in disks)
            {
                var keyValuePairs = disk
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var deviceId = keyValuePairs
                    .Single(k => k.StartsWith(COLUMN_WWN))
                    .Substring(COLUMN_WWN.Length + 2)
                    .TrimEnd('"');
                deviceId = deviceId == String.Empty ? null : deviceId;

                var name = keyValuePairs
                    .Single(k => k.StartsWith(COLUMN_NAME))
                    .Substring(COLUMN_NAME.Length + 2)
                    .TrimEnd('"');

                var size = UInt64.Parse
                (
                    keyValuePairs
                        .Single(k => k.StartsWith(COLUMN_SIZE))
                        .Substring(COLUMN_SIZE.Length + 2)
                        .TrimEnd('"')
                );

                yield return new Disk()
                {
                    DeviceID = deviceId,
                    Name = name,
                    Size = size
                };
            }
        }
    }
}
