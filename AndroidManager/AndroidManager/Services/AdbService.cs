using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Services
{
    public class AdbService : IAdbService
    {
        public string GetAdbExePath()
        {
            using var process = new Process();
            process.StartInfo = GetProcessStartInfo("powershell.exe", @"& {(Get-Command adb.exe).Path}");
            process.Start();
            process.WaitForExit(300);
            var output = process.StandardOutput.ReadToEnd();
            output = output.TrimEnd('\n').TrimEnd('\r');
            return output;
        }

        public void StartAdbServer()
        {
            StartAdbServer(GetAdbExePath());
        }

        public void StartAdbServer(string adbPath)
        {
            using var process = new Process();
            process.StartInfo = GetProcessStartInfo(adbPath, "start-server");
            process.Start();
            process.WaitForExit();
        }

        private ProcessStartInfo GetProcessStartInfo(string cmd, string args)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = cmd,
                Arguments = args,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            return startInfo;
        }
    }
}
