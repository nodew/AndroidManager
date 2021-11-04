using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AndroidManager.ViewModels
{
    public interface IDevicesViewModel
    {
        public ICommand RefreshConnectedDevicesCommand { get; }
        public ICommand ConnectToNewDeviceCommand { get; }
        public ICommand ClearInputCommand { get; }

        public ObservableCollection<DeviceData> Devices { get; }
        public DeviceData SelectedDeivce { get; set; }
        public string DeviceHost { get; set; }
        public string DevicePort { get; set; }

        public bool HasNoDevice { get; }
    }
}
