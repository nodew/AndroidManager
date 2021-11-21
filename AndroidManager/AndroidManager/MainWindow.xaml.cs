using AndroidManager.Models;
using AndroidManager.ViewModels;
using AndroidManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Vanara.PInvoke;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private static MainWindow _instance;

        public new static MainWindow Current => _instance;

        private MainWindow()
        {
            this.InitializeComponent();
            this.Title = "Android manager";
        }

        public static MainWindow GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MainWindow();
            }
            return _instance;
        }

        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToDevicesView();
        }

        public void NavigateToDevicesView()
        {
            mainFrame.Content = new DevicesView();
        }

        public void NavigateToDeviceDetailPage(DeviceData deviceData)
        {
            mainFrame.Navigate(typeof(DeviceDetailLandingView), deviceData);
        }

        public void NavigateToSettingsPage()
        {
            mainFrame.Navigate(typeof(SettingsView));
        }
    }
}
