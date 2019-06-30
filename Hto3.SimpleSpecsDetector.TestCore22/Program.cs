using System;

namespace Hto3.SimpleSpecsDetector.TestCore22
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

            //Processor
            Console.WriteLine("Processor.GetProcessorName: {0}", Processor.GetProcessorName());

            //Memory
            Console.WriteLine("Memory.GetAvailableMemory: {0} bytes", Memory.GetAvailableMemory());
            Console.WriteLine("Memory.GetInstalledMemory: {0} bytes", Memory.GetInstalledMemory());

            Console.WriteLine("\r\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
