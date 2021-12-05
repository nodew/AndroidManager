using AndroidManager.Services;
using AndroidManager.ViewModels;
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
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PackagesView : Page
    {

        private PackagesViewModel viewModel;

        public PackagesView()
        {
            this.viewModel = ServicesProvider.GetService<PackagesViewModel>();
            this.InitializeComponent();
        }

        private async void InstallPackageButton_Click(object sender, RoutedEventArgs e)
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Current);
            FileOpenPicker picker = new FileOpenPicker
                {
                    SuggestedStartLocation = PickerLocationId.HomeGroup
                };
            picker.FileTypeFilter.Add(".apk");
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                viewModel.InstallNewPackageCommand.Execute(file.Path);
            }
        }
    }
}
