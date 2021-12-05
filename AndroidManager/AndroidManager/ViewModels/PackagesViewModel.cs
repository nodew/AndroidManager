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

namespace AndroidManager.ViewModels
{
    public class PackagesViewModel : ViewModelBase
    {
        private PackageManager _packageManager;
        private ObservableCollection<Package> _packages;

        public PackagesViewModel(IAdbClient adbClient, DevicesViewModel devicesViewModel)
        {
            DeviceData device = devicesViewModel.CurrentSelectedDevice;
            _packageManager = new PackageManager(adbClient, device);
            _packages = new ObservableCollection<Package>();

            RefreshPackagesCommand = new RelayCommand(RefreshPackages);
            InstallNewPackageCommand = new AsyncRelayCommand<string>(InstallNewPackageAsync);

            LoadPackages();
        }

        public ICommand RefreshPackagesCommand;
        public IAsyncRelayCommand<string> InstallNewPackageCommand;

        public ObservableCollection<Package> Packages
        {
            get { return _packages; }
        }

        private void LoadPackages()
        {
            Packages.Clear();
            foreach (var item in _packageManager.Packages)
            {
                Packages.Add(new Package()
                {
                    Name = item.Key,
                    Path = item.Value
                });
            }
        }

        private void RefreshPackages()
        {
            _packageManager.RefreshPackages();
            LoadPackages();
        }

        private async Task InstallNewPackageAsync(string filepath)
        {
            try
            {
                await Task.Run(() => _packageManager.InstallPackage(filepath, false));
                RefreshPackages();
                WeakReferenceMessenger.Default.Send(new PackageInstalledEvent() { Filepath = filepath });
            }
            catch (PackageInstallationException ex)
            {
                WeakReferenceMessenger.Default.Send(ex);
            } 
        }
    }
}
