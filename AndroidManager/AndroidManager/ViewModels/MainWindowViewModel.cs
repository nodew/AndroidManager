using AndroidManager.Models;
using AndroidManager.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;

namespace AndroidManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Device> _devices;
        private readonly IAdbClient _adbClient;
        private readonly IAdbService _adbService;
        private readonly IDeviceService _deviceService;
        private readonly AppSettings _appSettings;
        private readonly ResourceLoader resourceLoader;

        private DeviceMonitor _monitor;
        private Device _currentSelectedDevice;
        private bool _adbServerIsRunning;
        private bool _noDevices;
        private string _deviceHost;
        private string _devicePort;
        private string _adbPath;
        private bool _loading;

        public MainWindowViewModel(
            IAdbClient adbClient,
            IAdbService adbService,
            IDeviceService deviceService,
            AppSettings appSettings
        )
        {
            _adbClient = adbClient;
            _adbService = adbService;
            _deviceService = deviceService;
            _appSettings = appSettings;

            resourceLoader = new ResourceLoader();
            _devices = new ObservableCollection<Device>();
            _noDevices = true;
            _currentSelectedDevice = null;
            _loading = false;

            RefreshConnectedDevicesCommand = new AsyncRelayCommand(RefreshConnectedDevicesAsync);
            ConnectToNewDeviceCommand = new RelayCommand(ConnectToNewDevice, CanConnectToNewDevice);
            ClearInputCommand = new RelayCommand(ClearInput);
            SelectDeviceCommand = new RelayCommand<Device>(SelectDevice);
            StartAdbServerCommand = new AsyncRelayCommand(StartAdbServerAsync, CanStartAdbServer);

            SetupAdbPath();
            UpdateAdbServerStatus();

            if (AdbServerIsRunning)
            {
                SetupMonitor();
            }
        }

        public ICommand RefreshConnectedDevicesCommand { get; }
        public ICommand ConnectToNewDeviceCommand { get; }
        public ICommand ClearInputCommand { get; }
        public ICommand SelectDeviceCommand { get; }
        public IAsyncRelayCommand StartAdbServerCommand { get; }

        public ObservableCollection<Device> Devices
        {
            get { return _devices; }
        }

        public bool NoDevices
        {
            get { return _noDevices; }
            set { SetProperty(ref _noDevices, value); }
        }

        public bool AdbServerIsRunning
        {
            get { return _adbServerIsRunning; }
            set { SetProperty(ref _adbServerIsRunning, value); }
        }

        public Device CurrentSelectedDevice
        {
            get { return _currentSelectedDevice; }
            set { SetProperty(ref _currentSelectedDevice, value); }
        }

        public string DeviceHost
        {
            get { return _deviceHost; }
            set { SetProperty(ref _deviceHost, value); }
        }

        public string DevicePort
        {
            get { return _devicePort; }
            set { SetProperty(ref _devicePort, value); }
        }

        private async Task LoadConnectedDevicesAsync()
        {
            if (_loading)
            {
                return;
            }
            _loading = true;
            Devices.Clear();
            try
            {
                var devices = _adbClient.GetDevices();
                foreach (var deviceData in devices)
                {
                    Device device = Device.FromDeviceData(deviceData);
                    if (device.State == DeviceState.Online)
                    {
                        try
                        {
                            DeviceDetail deviceDetail = await _deviceService.GetDeviceDetailAsync(device);
                            device.Name = deviceDetail.Name;
                            device.Model = deviceDetail.Model;
                            device.Brand = deviceDetail.Brand;
                            device.SdkVersion = deviceDetail.SdkVersion;
                            device.ReleaseVersion = deviceDetail.ReleaseVersion;
                        }
                        catch (Exception)
                        {
                            // Ignore
                        }
                    }

                    AddDevice(device);
                }

                NoDevices = devices.Count == 0;
            }
            catch (Exception)
            {
                AdbServerIsRunning = false;
            }
            finally
            {
                _loading = false;
            }
        }

        private void SetupAdbPath()
        {
            if (string.IsNullOrEmpty(_appSettings.AdbPath))
            {
                _adbPath = _adbService.GetAdbExePath();
            }
            else
            {
                _adbPath = _appSettings.AdbPath;
            }
        }

        private void SetupMonitor()
        {
            if (AdbServerIsRunning)
            {
                var socket = Factories.AdbSocketFactory(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort));
                _monitor = new DeviceMonitor(socket);
                _monitor.DeviceConnected += OnDeviceListUpdated;
                _monitor.DeviceDisconnected += OnDeviceListUpdated;
                _monitor.DeviceChanged += OnDeviceListUpdated;
                _monitor.Start();
            }
        }

        private async Task RefreshConnectedDevicesAsync()
        {
            UpdateAdbServerStatus();
            if (AdbServerIsRunning)
            {
                await LoadConnectedDevicesAsync();
            }
        }

        private async Task RefreshConnectedDevicesWithDelayAsync()
        {
            await Task.Delay(300);
            await RefreshConnectedDevicesAsync();
        }

        private void ConnectToNewDevice()
        {
            _adbClient.Connect(new DnsEndPoint(DeviceHost, int.Parse(DevicePort)));
            ClearInput();
        }

        private bool CanConnectToNewDevice()
        {
            if (string.IsNullOrWhiteSpace(DeviceHost))
            {
                var errorMessage = resourceLoader.GetString("DevicesPageInvalidHostMessage");
                WeakReferenceMessenger.Default.Send(new FailedToAddDeviceEvent(errorMessage));
                return false;
            }
            if (string.IsNullOrWhiteSpace(DevicePort) || !int.TryParse(DevicePort, out int _))
            {
                var errorMessage = resourceLoader.GetString("DevicesPageInvalidPortMessage");
                WeakReferenceMessenger.Default.Send(new FailedToAddDeviceEvent(errorMessage));
                return false;
            }
            return true;
        }

        private async Task StartAdbServerAsync()
        {
            if (string.IsNullOrEmpty(_adbPath) || !File.Exists(_adbPath))
            {
                WeakReferenceMessenger.Default.Send(new FailedToStartAdbServerEvent(resourceLoader.GetString("DevicesPageInvalidAdbExe")));
                return;
            }

            try
            {
                await _adbService.StartAdbServerAsync(_adbPath);
                await Task.Delay(300);
                UpdateAdbServerStatus();
                if (AdbServerIsRunning)
                {
                    SetupMonitor();
                    await RefreshConnectedDevicesWithDelayAsync();
                }
                else
                {
                    WeakReferenceMessenger.Default.Send(new FailedToStartAdbServerEvent(resourceLoader.GetString("DevicesPageStartAdbServerUnknownError")));
                }
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new FailedToStartAdbServerEvent(ex.Message));
            }
        }

        private bool CanStartAdbServer()
        {
            return !AdbServerIsRunning;
        }

        private void SelectDevice(Device device)
        {
            CurrentSelectedDevice = device;
        }

        private void ClearInput()
        {
            DeviceHost = string.Empty;
            DevicePort = string.Empty;
        }

        private void UpdateAdbServerStatus()
        {
            var status = AdbServer.Instance.GetStatus();
            AdbServerIsRunning = status.IsRunning;
        }

        private void AddDevice(Device device)
        {
            if (string.IsNullOrEmpty(device.Name))
            {
                device.Name = "Unknown";
            }

            Devices.Add(device);
        }

        private void OnDeviceListUpdated(object sender, DeviceDataEventArgs e)
        {
            //MainWindow.Current.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
            //{
            //    await RefreshConnectedDevicesWithDelayAsync();
            //});
        }
    }
}
