using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Detectors;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Hto3.SimpleSpecsDetector
{
    public static class HardwareDetector
    {
        private static IOsDetector osDetector;
        public static IOsDetector OsDetector
        {
            get
            {
                if (osDetector == null)
                    osDetector = DetectorFactory.CreateDetector<IOsDetector>();
                return osDetector;
            }
        }

        private static IMemoryDetector memoryDetector;
        public static IMemoryDetector MemoryDetector
        {
            get
            {
                if (memoryDetector == null)
                    memoryDetector = DetectorFactory.CreateDetector<IMemoryDetector>();
                return memoryDetector;
            }
        }

        private static IMotherboardDetector motherboardDetector;
        public static IMotherboardDetector MotherboardDetector
        {
            get
            {
                if (motherboardDetector == null)
                    motherboardDetector = DetectorFactory.CreateDetector<IMotherboardDetector>();
                return motherboardDetector;
            }
        }

        private static INetworkDetector networkDetector;
        public static INetworkDetector NetworkDetector
        {
            get
            {
                if (networkDetector == null)
                    networkDetector = DetectorFactory.CreateDetector<INetworkDetector>();
                return networkDetector;
            }
        }

        private static IPrinterDetector printerDetector;
        public static IPrinterDetector PrinterDetector
        {
            get
            {
                if (printerDetector == null)
                    printerDetector = DetectorFactory.CreateDetector<IPrinterDetector>();
                return printerDetector;
            }
        }

        private static IProcessorDetector processorDetector;
        public static IProcessorDetector ProcessorDetector
        {
            get
            {
                if (processorDetector == null)
                    processorDetector = DetectorFactory.CreateDetector<IProcessorDetector>();
                return processorDetector;
            }
        }

        private static ISoundDetector soundDetector;
        public static ISoundDetector SoundDetector
        {
            get
            {
                if (soundDetector == null)
                    soundDetector = DetectorFactory.CreateDetector<ISoundDetector>();
                return soundDetector;
            }
        }

        private static IStorageDetector storageDetector;
        public static IStorageDetector StorageDetector
        {
            get
            {
                if (storageDetector == null)
                    storageDetector = DetectorFactory.CreateDetector<IStorageDetector>();
                return storageDetector;
            }
        }

        private static IVideoDetector videoDetector;
        public static IVideoDetector VideoDetector
        {
            get
            {
                if (videoDetector == null)
                    videoDetector = DetectorFactory.CreateDetector<IVideoDetector>();
                return videoDetector;
            }
        }
    }
}
