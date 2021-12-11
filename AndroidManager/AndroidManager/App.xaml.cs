using AndroidManager.Services;
using AndroidManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SharpAdbClient;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        
        public IServiceProvider Services { get; }

        private Window m_window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
            SetupInitialWindow(hwnd, 1280, 720);
            
            m_window.Activate();
        }

        private static void SetupInitialWindow(IntPtr hwnd, int width, int height)
        {
            var dpi = PInvoke.User32.GetDpiForWindow(hwnd);
            float scalingFactor = (float)dpi / 96;
            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            _ = PInvoke.User32.SetWindowPos(hwnd, PInvoke.User32.SpecialWindowHandles.HWND_TOP,
                                        100, 100, width, height,
                                        PInvoke.User32.SetWindowPosFlags.SWP_SHOWWINDOW);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Global application settings
            services.AddSingleton<AppSettings>();

            // Register services
            services.AddSingleton<IAdbClient, AdbClient>();
            services.AddSingleton<IAdbService, AdbService>();
            services.AddSingleton<IDeviceService, DeviceService>();

            // Register view models
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<DevicesViewModel, DevicesViewModel>();
            services.AddTransient<DeviceDetailLandingViewModel, DeviceDetailLandingViewModel>();
            services.AddTransient<DeviceDetailViewModel, DeviceDetailViewModel>();
            services.AddTransient<PackagesViewModel, PackagesViewModel>();
            services.AddTransient<ProcessesViewModel, ProcessesViewModel>();
            services.AddTransient<FileExplorerViewModel, FileExplorerViewModel>();
            services.AddTransient<SettingsViewModel, SettingsViewModel>();

            return services.BuildServiceProvider();
        }

    }
}
