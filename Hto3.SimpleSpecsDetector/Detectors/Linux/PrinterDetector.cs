using Hto3.SimpleSpecsDetector.Contracts;
using Hto3.SimpleSpecsDetector.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Hto3.SimpleSpecsDetector.Detectors.Linux
{    
    internal class PrinterDetector : IPrinterDetector
    {
        public IEnumerable<PrinterDevice> GetPrinters()
        {
            if (!File.Exists("/usr/bin/lpstat"))
                yield break;

            var stdout = new StringBuilder();
            var processStartInfo = new ProcessStartInfo("lpstat", "-a -d");
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;

            using (var statProcess = Process.Start(processStartInfo))
            {
                var statProcessOutputDataReceived = new DataReceivedEventHandler((sender, e) => stdout.AppendLine(e.Data));
                statProcess.OutputDataReceived += statProcessOutputDataReceived;
                statProcess.BeginOutputReadLine();
                statProcess.WaitForExit();
            }

            var lpstatOutput = stdout.ToString().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var lastLpstatOutput = lpstatOutput[lpstatOutput.Length - 1];
            var defaultPrinterName = lastLpstatOutput.Substring(lastLpstatOutput.IndexOf(':') + 2);

            for (var i = 0; i < lpstatOutput.Length - 1; i++)
            {
                var printerName = lpstatOutput[i].Substring(0, lpstatOutput[i].IndexOf(' '));

                yield return new PrinterDevice()
                {
                    DefaultPrinter = defaultPrinterName == printerName,
                    Name = printerName
                };
            }
        }
    }
}
