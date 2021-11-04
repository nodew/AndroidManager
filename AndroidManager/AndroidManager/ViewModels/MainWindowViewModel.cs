using AndroidManager.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;

namespace AndroidManager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string currentView;
        private readonly ResourceLoader resourceLoader;

        public MainWindowViewModel()
        {
            currentView = View.Devices;

            resourceLoader = ResourceLoader.GetForViewIndependentUse();

            SelectedSideNavCommand = new RelayCommand<string>(SelectedSideNav, CanSelectedSideNav);
            
            MenuItems = new()
            {
                new MenuItem(View.Devices, resourceLoader.GetString("SideNavDevices"), "\xE8CC"),
                new MenuItem(View.Packages, resourceLoader.GetString("SideNavPackages"), "\xF158"),
                new MenuItem(View.Services, resourceLoader.GetString("SideNavServices"), "\xF22C"),
                new MenuItem(View.Explorer, resourceLoader.GetString("SideNavFileExplorer"), "\xED25")
            };
        }

        public string CurrentView
        {
            get { return currentView; }
            set { SetProperty(ref currentView, value); }
        }

        public List<MenuItem> MenuItems { get; }

        public ICommand SelectedSideNavCommand;

        private void SelectedSideNav(string view)
        {
            this.currentView = view;
            WeakReferenceMessenger.Default.Send(new ViewChangedEvent() { NextView = view });
        }

        private bool CanSelectedSideNav(string view)
        {
            return this.currentView != view;
        }
    }
}
