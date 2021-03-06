using AndroidManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.ViewModels
{
    public class DeviceDetailLandingViewModel : ViewModelBase
    {
        private readonly Device _selectedDevice;

        public DeviceDetailLandingViewModel(DevicesViewModel devicesViewModel)
        {
            _selectedDevice = devicesViewModel.CurrentSelectedDevice;
        }

        public string NameOfSelectedDevice
        {
            get => _selectedDevice.DisplayName;
        }
    }
}
