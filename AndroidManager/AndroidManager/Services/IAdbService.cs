using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidManager.Services
{
    public interface IAdbService
    {
        public string GetAdbExePath();

        public Task StartAdbServerAsync(CancellationToken cancellationToken = default);

        public Task StartAdbServerAsync(string adbPath, CancellationToken cancellationToken = default);
    }
}
