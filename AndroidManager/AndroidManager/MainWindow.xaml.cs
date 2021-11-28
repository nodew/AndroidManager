using AndroidManager.Services;
using AndroidManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Xaml;
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

        public MainWindow()
        {
            _appSettings = App.Current.Services.GetService<AppSettings>();
            resourceLoader = new ResourceLoader();
            this.Title = resourceLoader.GetString("AppName");
            this.InitializeComponent();

            ApplyLanguageSetting(_appSettings.Language);
            ApplyThemeSetting(_appSettings.Theme);
            _instance = this;
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

        public void ApplyLanguageSetting(string language)
        {
            ApplicationLanguages.PrimaryLanguageOverride =
                !string.IsNullOrEmpty(language) ? language : GlobalizationPreferences.Languages[0];
        }

        public void ApplyThemeSetting(string theme)
        {
            if (theme == "dark")
            {
                mainFrame.RequestedTheme = ElementTheme.Dark;
            }
            else if (theme == "light")
            {
                mainFrame.RequestedTheme = ElementTheme.Light;
            }
            else
            {
                mainFrame.RequestedTheme = GetSystemTheme();
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
    }
}
