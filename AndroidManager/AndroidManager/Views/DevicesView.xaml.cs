using AndroidManager.ViewModels;
using AndroidManager.Models;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DevicesView : Page
    {
        private DevicesViewModel viewModel;

        public DevicesView()
        {
            viewModel = App.Current.Services.GetService<DevicesViewModel>();
            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register<FailedToAddDeviceEvent>(this, async (o, e) =>
            {
                ClearInputBox();
                errorMessage.Text = e.ErrorMessage;
                await failedToAddNewDeviceDialog.ShowAsync();
            });
        }

        private void DeviceList_ItemClick(object sender, ItemClickEventArgs e)
        {
            DeviceData device = (DeviceData)e.ClickedItem;
            if (device.State.HasFlag(DeviceState.Online))
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
            deviceHostTextBox.Text = String.Empty;
            devicePortTextBox.Text = String.Empty;
        }
    }
}
