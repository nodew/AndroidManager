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
    public class DeviceService : IDeviceService
    {
        private readonly IAdbClient _adbClient;

        public DeviceService(IAdbClient adbClient)
        {
            _adbClient = adbClient;
        }

        public async Task<DeviceDetail> GetDeviceDetailAsync(DeviceData device, CancellationToken cancellationToken = default)
        {
            var receiver = new DeviceDetailReceiver() { TrimLines = true };
            await _adbClient.ExecuteRemoteCommandAsync("getprop", device, receiver, cancellationToken);
            
            return receiver.Detail;
        }
    }
}
