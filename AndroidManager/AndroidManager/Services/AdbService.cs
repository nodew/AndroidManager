using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task StartAdbServerAsync(CancellationToken cancellationToken = default)
        {
            await StartAdbServerAsync(GetAdbExePath(), cancellationToken);
        }

        /// <summary>
        /// The StartServer export in SharpAdbClient will hang
        /// </summary>
        /// <param name="adbPath">The customized adb path</param>
        /// <returns></returns>
        public async Task StartAdbServerAsync(string adbPath, CancellationToken cancellationToken = default)
        {
            using var process = new Process();
            process.StartInfo = GetProcessStartInfo(adbPath, "start-server");
            process.Start();
            await process.WaitForExitAsync(cancellationToken);
        }

        private static ProcessStartInfo GetProcessStartInfo(string cmd, string args)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = cmd,
                Arguments = args,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                StandardErrorEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
            };
            return startInfo;
        }
    }
}
