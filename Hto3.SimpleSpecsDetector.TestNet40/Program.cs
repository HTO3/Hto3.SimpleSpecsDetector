﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Hto3.SimpleSpecsDetector;

namespace Hto3.SimpleSpecsDetector.TestNet40
{
    class Program
    {
        static void Main(string[] args)
        {
            //Os
            Console.WriteLine("Os.GetOsVersionNumber: {0}", HardwareDetector.OsDetector.GetOsVersionNumber());
            Console.WriteLine("Os.GetOsVersionName: {0}", HardwareDetector.OsDetector.GetOsVersionName());
            Console.WriteLine("Os.GetInstalledFrameworkVersion: {0}", HardwareDetector.OsDetector.GetInstalledFrameworkVersion());
            Console.WriteLine("Os.GetSystemUpTime: {0}", HardwareDetector.OsDetector.GetSystemUpTime());
            Console.WriteLine("Os.GetKernelVersion: {0}", HardwareDetector.OsDetector.GetKernelVersion());

            //Video
            Console.WriteLine("Video.GetDisplayAdapterName: {0}", HardwareDetector.VideoDetector.GetDisplayAdapterName());
            Console.WriteLine("Video.GetVideoMemory: {0} bytes", HardwareDetector.VideoDetector.GetVideoMemory());
            Console.WriteLine("Video.GetCurrentVideoResolution: {0}", HardwareDetector.VideoDetector.GetCurrentVideoResolution());

            //Processor
            Console.WriteLine("Processor.GetProcessorName: {0}", HardwareDetector.ProcessorDetector.GetProcessorName());

            //Memory
            Console.WriteLine("Memory.GetFreeMemory: {0} bytes", HardwareDetector.MemoryDetector.GetFreeMemory());
            Console.WriteLine("Memory.GetVisibleMemory: {0} bytes", HardwareDetector.MemoryDetector.GetVisibleMemory());
            Console.WriteLine("Memory.GetInstalledMemory: {0} bytes", HardwareDetector.MemoryDetector.GetInstalledMemory());

            //Motherboard
            Console.WriteLine("Motherboard.GetModel: {0}", HardwareDetector.MotherboardDetector.GetModel());
            Console.WriteLine("Motherboard.GetVendorName: {0}", HardwareDetector.MotherboardDetector.GetVendorName());
            Console.WriteLine("Motherboard.GetBIOSVersion: {0}", HardwareDetector.MotherboardDetector.GetBIOSVersion());

            //Storage
            foreach (var disk in HardwareDetector.StorageDetector.GetDisks())
                Console.WriteLine("Storage.GetDisks: {0}", disk);

            //Printers
            foreach (var printer in HardwareDetector.PrinterDetector.GetPrinters())
                Console.WriteLine("Printer.GetPrinters: {0}", printer);

            //Sound cards
            foreach (var soundCard in HardwareDetector.SoundDetector.GetSoundCards())
                Console.WriteLine("Sound.GetSoundCards: {0}", soundCard);

            //Network cards
            foreach (var networkAdapter in HardwareDetector.NetworkDetector.GetNetworkCards())
                Console.WriteLine("Network.GetNetworkCards: {0}", networkAdapter);

            Console.WriteLine("\r\nPress any key to exit...");
            Console.Read();
        }
    }
}
