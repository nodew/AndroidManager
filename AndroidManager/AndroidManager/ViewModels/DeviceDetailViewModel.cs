using AndroidManager.Models;
using AndroidManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.ViewModels
{
    public class DeviceDetailViewModel : ViewModelBase
    {
        private readonly Device _selectedDevice;
        private readonly IDeviceService _deviceService;
        private DeviceDetail _deviceDetail;

        public DeviceDetailViewModel(DevicesViewModel devicesViewModel, IDeviceService deviceService)
        {
            _selectedDevice = devicesViewModel.CurrentSelectedDevice;
            _deviceService = deviceService;
            _ = LoadDeviceDetailAsync();
        }

        public DeviceDetail DeviceDetail
        {
            get { return _deviceDetail; }
            set { SetProperty(ref _deviceDetail, value); }
        }

        private async Task LoadDeviceDetailAsync()
        {
            DeviceDetail = await _deviceService.GetDeviceDetailAsync(_selectedDevice);
        }
    }
}
