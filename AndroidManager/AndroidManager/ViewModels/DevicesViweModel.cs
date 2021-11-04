using AndroidManager.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AndroidManager.ViewModels
{
    public class DevicesViweModel : ViewModelBase, IDevicesViewModel
    {
        private readonly ObservableCollection<DeviceData> _devices;
        private AdbClient _adbClient;
        private DeviceData _selectedDeivce;

        private string _deviceHost;
        private string _devicePort;

        public DevicesViweModel()
        {
            _adbClient = new AdbClient();
            _devices = new ObservableCollection<DeviceData>();

            RefreshConnectedDevicesCommand = new RelayCommand(RefreshConnectedDevices);
            ConnectToNewDeviceCommand = new RelayCommand(ConnectToNewDevice, CanConnectToNewDevice);
            ClearInputCommand = new RelayCommand(ClearInput);

            LoadConnectedDevices();
        }

        public ICommand RefreshConnectedDevicesCommand { get; }
        public ICommand ConnectToNewDeviceCommand { get; }
        public ICommand ClearInputCommand { get; }

        public ObservableCollection<DeviceData> Devices
        {
            get { return _devices; }
        }

        public bool HasNoDevice
        {
            get { return _devices.Count == 0; }
        }

        public DeviceData SelectedDeivce 
        { 
            get {  return _selectedDeivce; }
            set { SetProperty(ref _selectedDeivce, value); }
        }

        public string DeviceHost { 
            get {  return _deviceHost; } 
            set { SetProperty(ref _deviceHost, value); } 
        }

        public string DevicePort { 
            get { return _devicePort; } 
            set { SetProperty(ref _devicePort, value); }
        }

        private void LoadConnectedDevices()
        {
            var devices = _adbClient.GetDevices();
            foreach (var device in devices)
            {
                Devices.Add(device);
            }
        }

        private void RefreshConnectedDevices()
        {
            Devices.Clear();
            LoadConnectedDevices();
        }

        private void ConnectToNewDevice()
        {
            _adbClient.Connect(new DnsEndPoint(DeviceHost, int.Parse(DevicePort)));
            
            int currentDevicesCount = Devices.Count;
            RefreshConnectedDevices();
            if (Devices.Count == currentDevicesCount)
            {
                var errorMessage = $"Failed to connect to the device {DeviceHost}:{DevicePort}, please check your settings";
                WeakReferenceMessenger.Default.Send(new FailedToAddDeviceEvent() { ErrorMessage = errorMessage });
            } 
            else
            {
                ClearInput();
            }
        }

        private bool CanConnectToNewDevice()
        {
            if (string.IsNullOrWhiteSpace(DeviceHost))
            {
                var errorMessage = "Host can't be empty";
                WeakReferenceMessenger.Default.Send(new FailedToAddDeviceEvent() { ErrorMessage = errorMessage });
                return false;
            }
            if (string.IsNullOrWhiteSpace(DevicePort) || !int.TryParse(DevicePort, out int _))
            {
                var errorMessage = "Invalid port";
                WeakReferenceMessenger.Default.Send(new FailedToAddDeviceEvent() { ErrorMessage = errorMessage });
                return false;
            }
            return true;
        }

        private void ClearInput()
        {
            DeviceHost = string.Empty;
            DevicePort = string.Empty;
        }
    }
}
