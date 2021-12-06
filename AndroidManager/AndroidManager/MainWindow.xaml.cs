using AndroidManager.Services;
using AndroidManager.ViewModels;
using AndroidManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SharpAdbClient;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.System.UserProfile;

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
        private readonly AppSettings _appSettings;
        private readonly ResourceLoader resourceLoader;
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            _appSettings = ServicesProvider.GetService<AppSettings>();
            viewModel = ServicesProvider.GetService<MainWindowViewModel>();
            resourceLoader = new ResourceLoader();
            this.Title = resourceLoader.GetString("AppName");
            this.InitializeComponent();

            ApplyLanguageSetting(_appSettings.Language);
            ApplyThemeSetting(_appSettings.Theme);
            _instance = this;
        }

        private void ContentFrameLoaded(object sender, RoutedEventArgs e)
        {
            NavigateToDeviceView();
        }

        public void NavigateToDeviceView()
        {
            mainContent.Content = new DeviceDetailLandingView();
        }

        public void NavigateToDeviceDetailPage(DeviceData deviceData)
        {
            mainContent.Navigate(typeof(DeviceDetailLandingView), deviceData);
        }

        public void NavigateToSettingsPage()
        {
            mainContent.Navigate(typeof(SettingsView));
        }

        public void ApplyLanguageSetting(string language)
        {
            ApplicationLanguages.PrimaryLanguageOverride =
                !string.IsNullOrEmpty(language) ? language : GlobalizationPreferences.Languages[0];
        }

        public void ApplyThemeSetting(string theme)
        {
            if (theme == "dark")
            {
                mainWindow.RequestedTheme = ElementTheme.Dark;
            }
            else if (theme == "light")
            {
                mainWindow.RequestedTheme = ElementTheme.Light;
            }
            else
            {
                mainWindow.RequestedTheme = GetSystemTheme();
            }
        }

        private static ElementTheme GetSystemTheme()
        {
            var uiSettings = new Windows.UI.ViewManagement.UISettings();
            var color = uiSettings.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background);
            if (color == Colors.Black)
            {
                return ElementTheme.Dark;
            }
            return ElementTheme.Light;
        }

        private void MainNavSelectionChanged(object sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                NavigateToSettingsPage();
                return;
            }
            viewModel.SelectDeviceCommand.Execute(args.SelectedItem);
            NavigateToDeviceView();
        }
    }
}
