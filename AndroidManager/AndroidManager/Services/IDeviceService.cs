using AndroidManager.Models;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidManager.Services
{
    public interface IDeviceService
    {
        public Task<DeviceDetail> GetDeviceDetailAsync(DeviceData device, CancellationToken cancellationToken = default);
    }
}
