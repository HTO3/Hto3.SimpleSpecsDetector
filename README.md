<img alt="logo" width="150" height="150" src="nuget-logo.png">

Hto3.SimpleSpecsDetector
========================================

#### Nuget Package
[![Hto3.SimpleSpecsDetector](https://img.shields.io/nuget/v/Hto3.SimpleSpecsDetector.svg)](https://www.nuget.org/packages/Hto3.SimpleSpecsDetector/)

Fully managed .NET library to detect the specs of the hardware, available in .NET Framework or .NET Core flavors (.NET Standard 2.0). This library intends to keep direct and simple, don't expect to retrieve all geek info like HWiNFO, AIDA64 or Speccy provides.

This library is ideal for obtaining simple information from PCs in Windows environment, perhaps for a troubleshooting or inventory, or simply to use in the log of your application.

Supported Windows Versions
--------
-   Windows XP (.NET 4.0)
-   Windows Vista
-   Windows 7
-   Windows 8
-   Windows 8.1
-   Windows 10
-   ~**Future proof** 🐱‍👤

Supported .NET Versions
--------
-   .NET Framework 4.x
-   .NET Core 2.x (through .NET Standard 2.0)

Features
--------

#### Os
`GetWindowsVersionNumber` get the correct Windows version.

`GetWindowsVersionName` get the Windows version name.

#### Processor
`GetProcessorName` get the processor name.

#### Video
`GetDisplayAdapterName` get the display adapter name.

`GetVideoMemory` get amount of memory of the display adapter. Result in bytes. 

#### Memory
`GetAvailableMemory` get the available phisical memory in bytes.

`GetInstalledMemory` get the amount of installed phisical memory in bytes.