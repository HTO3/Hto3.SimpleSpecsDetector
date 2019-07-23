using System;
using System.Collections.Generic;
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
            Console.WriteLine("Os.GetWindowsVersion: {0}", Os.GetWindowsVersionNumber());
            Console.WriteLine("Os.GetWindowsVersionName: {0}", Os.GetWindowsVersionName());

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

            //Storage
            foreach (var disk in Storage.GetDisks())
                Console.WriteLine("Storage.GetDisks: {0}", disk);

            Console.WriteLine("\r\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
