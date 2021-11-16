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
    public class DevicesViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DeviceData> _devices;
        private readonly AdbClient _adbClient;
        private DeviceData _currentSelectedDeivce;

        private string _deviceHost;
        private string _devicePort;
        private DevicesPageState _pageState;

        public DevicesViewModel(AdbClient adbClient)
        {
            _adbClient = adbClient;
            _devices = new ObservableCollection<DeviceData>();
            _pageState = DevicesPageState.NoRunningServer;
            _currentSelectedDeivce = null;

            RefreshConnectedDevicesCommand = new RelayCommand(RefreshConnectedDevices);
            ConnectToNewDeviceCommand = new RelayCommand(ConnectToNewDevice, CanConnectToNewDevice);
            ClearInputCommand = new RelayCommand(ClearInput);
            SelectDeviceCommand = new RelayCommand<DeviceData>(SelectDevice);

            if (AdbServer.Instance.GetStatus().IsRunning)
            {
                LoadConnectedDevices();
            }
        }

        public ICommand RefreshConnectedDevicesCommand { get; }
        public ICommand ConnectToNewDeviceCommand { get; }
        public ICommand ClearInputCommand { get; }
        public ICommand SelectDeviceCommand { get; }

        public ObservableCollection<DeviceData> Devices
        {
            get { return _devices; }
        }

        public bool HasDevices
        {
            get { return _devices.Count > 0; }
        }

        public DevicesPageState PageState
        {
            get { return _pageState; }
            set {  SetProperty(ref _pageState, value); }
        }

        public DeviceData CurrentSelectedDeivce 
        { 
            get {  return _currentSelectedDeivce; }
            set { SetProperty(ref _currentSelectedDeivce, value); }
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
            Devices.Clear();
            try
            {
                var devices = _adbClient.GetDevices();
                foreach (var device in devices)
                {
                    Devices.Add(device);
                }

                if (Devices.Count > 0)
                {
                    PageState = DevicesPageState.HasDevices;
                }
                else
                {
                    PageState = DevicesPageState.NoDevice;
                }
            }
            catch (Exception)
            {
                PageState = DevicesPageState.NoRunningServer;
            }
        }

        private void RefreshConnectedDevices()
        {
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

        private void SelectDevice(DeviceData device)
        {
            CurrentSelectedDeivce = device;
        }

        private void ClearInput()
        {
            DeviceHost = string.Empty;
            DevicePort = string.Empty;
        }
    }
}
