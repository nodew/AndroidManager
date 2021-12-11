using AndroidManager.Services;
using AndroidManager.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using SharpAdbClient;
using System;
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
        private AppWindowTitleBar _titleBar;

        public MainWindow()
        {
            _appSettings = ServicesProvider.GetService<AppSettings>();
            resourceLoader = new ResourceLoader();
            this.InitializeComponent();
            this.Title = resourceLoader.GetString("AppName");

            ApplyLanguageSetting(_appSettings.Language);
            ApplyThemeSetting(_appSettings.Theme);
            SetupTitleBar();

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
                mainWindowArea.RequestedTheme = ElementTheme.Dark;
            }
            else if (theme == "light")
            {
                mainWindowArea.RequestedTheme = ElementTheme.Light;
            }
            else
            {
                mainWindowArea.RequestedTheme = GetSystemTheme();
            }
        }

        private void SetupTitleBar()
        {
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
                AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
                _titleBar = appWindow.TitleBar;
                _titleBar.ExtendsContentIntoTitleBar = true;
                _titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                _titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleIcon.Margin = new Thickness(_titleBar.LeftInset + 12, 0, 8, 0);
                titleText.Margin = new Thickness(0, 0, _titleBar.RightInset, 0);
                titleBar.SizeChanged += HandleTitleBarSizeChanged;
                titleBar.Visibility = Visibility.Visible;
            }
        }

        private void HandleTitleBarSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_titleBar != null)
            {
                _titleBar.SetDragRectangles(
                    new Windows.Graphics.RectInt32[] { 
                        new Windows.Graphics.RectInt32()
                        {
                            Width = (int) e.NewSize.Width,
                            Height = (int)e.NewSize.Height
                        }});
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
