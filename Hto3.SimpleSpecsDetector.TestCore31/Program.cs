using System;

namespace Hto3.SimpleSpecsDetector.TestCore31
{
    class Program
    {
        static void Main(string[] args)
        {
            //Os
            Console.WriteLine("Os.GetWindowsVersion: {0}", Os.GetWindowsVersionNumber());
            Console.WriteLine("Os.GetWindowsVersionName: {0}", Os.GetWindowsVersionName());
            Console.WriteLine("Os.GetInstalledFrameworkVersion: {0}", Os.GetInstalledFrameworkVersion());
            Console.WriteLine("Os.GetSystemUpTime: {0}", Os.GetSystemUpTime());

            //Video
            Console.WriteLine("Video.GetDisplayAdapterName: {0}", Video.GetDisplayAdapterName());
            Console.WriteLine("Video.GetVideoMemory: {0} bytes", Video.GetVideoMemory());
            Console.WriteLine("Video.GetCurrentVideoResolution: {0}", Video.GetCurrentVideoResolution());

            //Processor
            Console.WriteLine("Processor.GetProcessorName: {0}", Processor.GetProcessorName());

            //Memory
            Console.WriteLine("Memory.GetFreeMemory: {0} bytes", Memory.GetFreeMemory());
            Console.WriteLine("Memory.GetVisibleMemory: {0} bytes", Memory.GetVisibleMemory());
            Console.WriteLine("Memory.GetInstalledMemory: {0} bytes", Memory.GetInstalledMemory());

            //Motherboard
            Console.WriteLine("Motherboard.GetModel: {0}", Motherboard.GetModel());
            Console.WriteLine("Motherboard.GetVendorName: {0}", Motherboard.GetVendorName());
            Console.WriteLine("Motherboard.GetBIOSVersion: {0}", Motherboard.GetBIOSVersion());

            //Storage
            foreach (var disk in Storage.GetDisks())
                Console.WriteLine("Storage.GetDisks: {0}", disk);

            //Printers
            foreach (var printer in Printer.GetPrinters())
                Console.WriteLine("Printer.GetPrinters: {0}", printer);

            //Sound cards
            foreach (var soundCard in Sound.GetSoundCards())
                Console.WriteLine("Sound.GetSoundCards: {0}", soundCard);

            //Network cards
            foreach (var networkAdapter in Network.GetNetworkCards())
                Console.WriteLine("Network.GetNetworkCards: {0}", networkAdapter);

            Console.WriteLine("\r\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
