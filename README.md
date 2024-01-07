![logo](https://raw.githubusercontent.com/HTO3/Hto3.SimpleSpecsDetector/master/nuget-logo-small.png)

Hto3.SimpleSpecsDetector
========================================

[![License](https://img.shields.io/github/license/HTO3/Hto3.SimpleSpecsDetector)](https://github.com/HTO3/Hto3.SimpleSpecsDetector/blob/master/LICENSE)
[![Hto3.SimpleSpecsDetector](https://img.shields.io/nuget/v/Hto3.SimpleSpecsDetector.svg)](https://www.nuget.org/packages/Hto3.SimpleSpecsDetector/)
[![Downloads](https://img.shields.io/nuget/dt/Hto3.SimpleSpecsDetector)](https://www.nuget.org/stats/packages/Hto3.SimpleSpecsDetector?groupby=Version)
[![Build Status](https://github.com/HTO3/Hto3.SimpleSpecsDetector/actions/workflows/publish.yml/badge.svg)](https://github.com/HTO3/Hto3.SimpleSpecsDetector/actions/workflows/publish.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d28742145d944649a646177e72be0c1d)](https://www.codacy.com/gh/HTO3/Hto3.SimpleSpecsDetector/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=HTO3/Hto3.SimpleSpecsDetector&amp;utm_campaign=Badge_Grade)

Fully managed .NET library to detect the specs of the hardware, available in .NET Framework or .NET Core flavors (.NET Standard). This library intends to keep direct and simple, don't expect to retrieve all geek info like HWiNFO, AIDA64 or Speccy provides.

This library is ideal for obtaining simple information from Desktops and Servers, perhaps for a troubleshooting or inventory, or simply to use in the log of your application.

##### Example

![example](https://raw.githubusercontent.com/HTO3/Hto3.SimpleSpecsDetector/master/example.png) 

Supported Windows Versions
--------
-   Windows 11
-   Windows Server 2022
-   Windows 10
-   Windows Server 2019
-   Windows Server 2016
-   Windows 8.1
-   Windows Server 2012 R2
-   Windows 8
-   Windows Server 2012
-   Windows 7
-   Windows Server 2008 R2
-   Windows Server 2008

Discontinuated support. Use up to version [1.2.3](https://www.nuget.org/packages/Hto3.SimpleSpecsDetector/1.2.3)

-   ~~Windows Vista~~
-   ~~Windows Server 2003 R2~~
-   ~~Windows Server 2003~~
-   ~~Windows XP 64-Bit Edition~~
-   ~~Windows XP~~

Supported Linux Versions
--------
-   Same as .NET supported list ([link](https://docs.microsoft.com/dotnet/core/install/linux)).

Supported .NET Versions
--------
-   .NET Framework 4.6.1 to 4.8.1
-   .NET Core 3.x (through .NET Standard)
-   .NET 5.0
-   .NET 6.0
-   .NET 7.0
-   .NET 8.0

Related projects:
----------------
- https://openhardwaremonitor.org/ The Open Hardware Monitor supports most hardware monitoring chips found on todays mainboards. The CPU temperature can be monitored by reading the core temperature sensors of Intel and AMD processors. The sensors of ATI and Nvidia video cards as well as SMART hard drive temperature can be displayed. There is support from .NET Framework version 4.5 and above, besides the original project, you can find forks spread out there, for example supporting .NET Standard [[1](https://www.nuget.org/packages/OpenHardwareMonitorLibCore/)]. Open Hardware Monitor is much more complete compared with this project, specialized to retrieve sensors data.


Features
--------

### Os
- `GetOsVersionNumber` Windows: Get the correct Windows version following the Microsoft table (https://docs.microsoft.com/en-us/windows/win32/sysinfo/operating-system-version).
   Linux: Get the VERSION_ID value.
- `GetOsVersionName` Get the OS version name.
- `GetInstalledFrameworkVersion` Get the higher .NET Framework version installed on machine. This method can detect starting from 4.0 version.
- `GetSystemUpTime` Get system up time.
- `GetKernelVersion` Get the OS kernel version.

### Processor
- `GetProcessorName` Get the processor name.
- `GetProcessorUsage` Mensure and get the current processor percent usage, where 0 is no load and 1 is full load. Default mensure time is 1 second.

### Video
- `GetDisplayAdapterName` Get the display adapter name.
- `GetVideoMemory` Get amount of memory of the display adapter. Result in bytes. 
- `GetCurrentVideoResolution` Get the resolution in pixels of the current display in use (focused).

### Memory
- `GetFreeMemory` Number of bytes of physical memory currently unused and available.
- `GetInstalledMemory` Get the amount of installed physical memory in bytes.
- `GetVisibleMemory` The total amount of physical memory (in Bytes) available to the OperatingSystem. This value does not necessarily indicate the true amount of physical memory, but what is reported to the OperatingSystem as available to it.

### Motherboard
- `GetVendorName` Get the vendor name.
- `GetModel` Get the motherboard model.
- `GetBIOSVersion` Get the BIOS version.

### Storage
- `GetDisks` Get all connected hard disks. The information available is the hard disk name and size.

### Sound
- `GetSoundCards` Get all connected sound cards.

### Printer
- `GetPrinters` Get all installed printers.

### Network
- `GetNetworkCards` Get all connected network cards.
- `GetNetworkThroughput` Mensure 1 second and get the current network traffic throughput. Result in Bytes.

Sample App
----------

```C#
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
        Console.WriteLine("Processor.GetProcessorUsage: {0}%", HardwareDetector.ProcessorDetector.GetProcessorUsage().Result * 100);

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
        Console.WriteLine("Network.GetNetworkThroughput: {0}", HardwareDetector.NetworkDetector.GetNetworkThroughput(NetworkInterface.GetAllNetworkInterfaces().Last().Description).Result);
    }
}
```