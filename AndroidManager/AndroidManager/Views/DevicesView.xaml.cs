using AndroidManager.ViewModels;
using AndroidManager.Models;
using AndroidManager.Services;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Threading.Tasks;
using SharpAdbClient;
using Windows.ApplicationModel.Resources;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicesView : Page
    {
        private readonly DevicesViewModel viewModel;
        private readonly ResourceLoader resourceLoader;

        public DevicesView()
        {
            viewModel = ServicesProvider.GetService<DevicesViewModel>();
            resourceLoader = new ResourceLoader();
            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register<FailedToAddDeviceEvent>(this, HandleFailedToAddDeviceEvent);
            WeakReferenceMessenger.Default.Register<FailedToStartAdbServerEvent>(this, HandleFailedToStartAdbServer);
        }

        private void DeviceList_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeviceData device = (DeviceData)e.ClickedItem;
            if (device.State == DeviceState.Online)
            {
                viewModel.SelectDeviceCommand.Execute(device);
                MainWindow.Current.NavigateToDeviceDetailPage(device);
            }
        }

        private async void ConnectToNewDevice(object sender, RoutedEventArgs e)
        {
            ContentDialogResult result = await addNewDeviceDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                viewModel.ConnectToNewDeviceCommand.Execute(null);
            }
            else
            {
                ClearInputBox();
                viewModel.ClearInputCommand.Execute(null);
            }
        }

        private void ClearInputBox()
        {
            deviceHostTextBox.Text = string.Empty;
            devicePortTextBox.Text = string.Empty;
        }

        private void GotoSettingsPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.NavigateToSettingsPage();
        }

        private async void HandleFailedToAddDeviceEvent(object sender, FailedToAddDeviceEvent e)
        {
            ClearInputBox();
            failureDialog.Content = e.ErrorMessage;
            failureDialog.Title = resourceLoader.GetString("DevicesPageConnectFailedDialogTitle");
            await failureDialog.ShowAsync();
        }

        private async void HandleFailedToStartAdbServer(object sender, FailedToStartAdbServerEvent e)
        {
            failureDialog.Content = e.ErrorMessage;
            failureDialog.Title = resourceLoader.GetString("DevicesPageStartAdbServerDialogTitle");
            await failureDialog.ShowAsync();
        }

        private async void FailedToViewDetails()
        {
            failureDialog.Content = resourceLoader.GetString("DevicesPageUnauthorizedText");
            failureDialog.Title = resourceLoader.GetString("DevicesPageUnauthorizedTitle");
            await failureDialog.ShowAsync();
        }
    }
}
