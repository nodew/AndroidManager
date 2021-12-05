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
            var receiver = new DeviceDetailReceiver();
            await _adbClient.ExecuteRemoteCommandAsync("getprop", device, receiver, cancellationToken);
            
            return receiver.Detail;
        }

        public async Task<List<string>> ListSystemPackagesAsync(DeviceData device, CancellationToken cancellationToken = default)
        {
            var receiver = new PackageReceiver();
            await _adbClient.ExecuteRemoteCommandAsync("pm list packages -s", device, receiver, cancellationToken);
            return receiver.Packages;
        }

        public async Task<List<string>> ListThirdPartyPackagesAsync(DeviceData device, CancellationToken cancellationToken = default)
        {
            var receiver = new PackageReceiver();
            await _adbClient.ExecuteRemoteCommandAsync("pm list packages -3", device, receiver, cancellationToken);
            return receiver.Packages;
        }
    }
}
