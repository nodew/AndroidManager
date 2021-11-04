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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private readonly List<NavNode> nodes = new()
        {
            new NavNode() { Name = View.Devices, Page = typeof(DevicesView) },
            new NavNode() { Name = View.Packages, Page = typeof(PackagesView) },
            new NavNode() { Name = View.Services, Page = typeof(ServicesView) },
            new NavNode() { Name = View.Explorer, Page = typeof(FileExplorerView) },
            new NavNode() { Name = View.Settings, Page = typeof(SettingsView) }
        };

        private readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            viewModel = App.Current.Services.GetService<MainWindowViewModel>();
            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register<ViewChangedEvent>(this, (r, m) => NavigateToNextPage(m.NextView));
        }

        private void NavigateToNextPage(string viewName)
        {
            Type page = nodes.Where(x => x.Name == viewName).FirstOrDefault().Page;
            if (page != null)
            {
                content.Navigate(page);
            }
        }

        private void SideNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args != null)
            {
                string view;
                if (args.IsSettingsSelected)
                {
                    view = View.Settings;
                }
                else
                {
                    view = ((MenuItem)args.SelectedItem).Name;
                }
                viewModel.SelectedSideNavCommand.Execute(view);
            }
        }

        private void SideNav_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToNextPage(viewModel.CurrentView);
        }

        private record NavNode
        {
            public string Name;
            public Type Page;
        }
    }
}
