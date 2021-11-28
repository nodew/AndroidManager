﻿using AndroidManager.Models;
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
        private bool _installing;

        public PackagesViewModel(AdbClient adbClient, DevicesViewModel devicesViweModel)
        {
            DeviceData device = devicesViweModel.CurrentSelectedDevice;
            _packageManager = new PackageManager(adbClient, device);
            _packages = new ObservableCollection<Package>();

            RefreshPackagesCommand = new RelayCommand(RefreshPackages);
            InstallNewPackageCommand = new RelayCommand<string>(InstallNewPackage);

            LoadPackages();
        }

        public ICommand RefreshPackagesCommand;
        public ICommand InstallNewPackageCommand;

        public ObservableCollection<Package> Packages
        {
            get { return _packages; }
        }

        public bool Installing
        {
            get { return _installing; }
            set { SetProperty(ref _installing, value); }
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

        private void InstallNewPackage(string filepath)
        {
            Installing = true;
            MainWindow.Current.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
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
                finally
                {
                    Installing = false;
                }
            });
            
        }
    }
}
