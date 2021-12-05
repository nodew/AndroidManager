using AndroidManager.Services;
using AndroidManager.ViewModels;
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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DeviceDetailLandingView : Page
    {
        private readonly Dictionary<string, Type> viewMapping = new()
        {
            { "General", typeof(DeviceDetailView) },
            { "Packages", typeof(PackagesView) },
            { "Processes", typeof(ProcessesView) },
            { "FileExplorer", typeof(FileExplorerView) }
        };

        private readonly DeviceDetailLandingViewModel viewModel;

        public DeviceDetailLandingView()
        {
            viewModel = ServicesProvider.GetService<DeviceDetailLandingViewModel>();
            this.InitializeComponent();
        }

        private void DeviceDetailNav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            MainWindow.Current.NavigateToDevicesView();
        }

        private void DeviceDetailNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem item = (NavigationViewItem)args.SelectedItem;
            if (item != null)
            {
                string page = item.Tag.ToString();
                if (!string.IsNullOrEmpty(page) && viewMapping.TryGetValue(page, out Type nextPage))
                {
                    detailContent.Navigate(nextPage);
                }
            }
        }

        private void DeviceDetailNav_Loaded(object sender, RoutedEventArgs e)
        {
            detailContent.Navigate(typeof(DeviceDetailView));
            deviceDetailNav.SelectedItem = deviceDetailNav.MenuItems[0];
        }
    }
}
