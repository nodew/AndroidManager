using AndroidManager.Models;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Input;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Dispatching;
using AndroidManager.Services;

namespace AndroidManager.ViewModels
{
    public class PackagesViewModel : ViewModelBase
    {
        private readonly PackageManager _packageManager;
        private readonly IDeviceService _deviceService;
        private readonly DeviceData _selectedDevice;
        private readonly ObservableCollection<Package> _systemPackages;
        private readonly ObservableCollection<Package> _thirdPartyPackages;

        private PackagesViewType _viewType;

        public PackagesViewModel(
            IAdbClient adbClient, 
            IDeviceService deviceService,
            MainWindowViewModel mainWindowViewModel
        )
        {
            DeviceData device = mainWindowViewModel.CurrentSelectedDevice;
            _deviceService = deviceService;
            _selectedDevice = device;
            _viewType = PackagesViewType.ThirdParty;
            _packageManager = new PackageManager(adbClient, device);
            _systemPackages = new ObservableCollection<Package>();
            _thirdPartyPackages = new ObservableCollection<Package>();

            RefreshPackagesCommand = new AsyncRelayCommand(RefreshPackagesAsync);
            InstallNewPackageCommand = new AsyncRelayCommand<string>(InstallNewPackageAsync);
            SwitchTabCommand = new RelayCommand<string>(SwitchTab);

            _= LoadThirdPartyPackagesAsync();
            _ = LoadSystemPackagesAsync();
        }

        public IAsyncRelayCommand RefreshPackagesCommand;
        public IAsyncRelayCommand<string> InstallNewPackageCommand;
        public ICommand SwitchTabCommand;

        public ObservableCollection<Package> SystemPackages
        {
            get { return _systemPackages; }
        }

        public ObservableCollection<Package> ThirdPartyPackages
        {
            get { return _thirdPartyPackages; }
        }

        public PackagesViewType ViewType
        {
            get { return _viewType; }
            set { SetProperty(ref _viewType, value); }
        }

        private async Task LoadSystemPackagesAsync()
        {
            SystemPackages.Clear();

            var systemPackages = await _deviceService.ListSystemPackagesAsync(_selectedDevice);

            foreach (var item in systemPackages)
            {
                SystemPackages.Add(new Package()
                {
                    Name = item,
                    IsThirdParty = false
                });
            }
        }

        private async Task LoadThirdPartyPackagesAsync()
        {
            ThirdPartyPackages.Clear();

            var thirdPartyPackages = await _deviceService.ListThirdPartyPackagesAsync(_selectedDevice);

            foreach (var item in thirdPartyPackages)
            {
                ThirdPartyPackages.Add(new Package()
                {
                    Name = item,
                    IsThirdParty = true
                });
            }
        }

        private async Task RefreshPackagesAsync()
        {
            _packageManager.RefreshPackages();
            if (ViewType == PackagesViewType.ThirdParty)
            {
                await LoadThirdPartyPackagesAsync();
            }
            else
            {
                await LoadSystemPackagesAsync();
            }
        }

        private async Task InstallNewPackageAsync(string filepath)
        {
            try
            {
                await Task.Run(() => _packageManager.InstallPackage(filepath, false));
                await RefreshPackagesAsync();
                WeakReferenceMessenger.Default.Send(new PackageInstalledEvent() { Filepath = filepath });
            }
            catch (PackageInstallationException ex)
            {
                WeakReferenceMessenger.Default.Send(ex);
            } 
        }

        private void SwitchTab(string tab)
        {
            if(Enum.TryParse(tab, out PackagesViewType viewType)) 
            {
                ViewType = viewType;
            }
        }

        public enum PackagesViewType
        {
            System,
            ThirdParty,
        }
    }
}
