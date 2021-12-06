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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AndroidManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        public SettingsViewModel viewModel;
        public SettingsView()
        {
            viewModel = ServicesProvider.GetService<SettingsViewModel>();
            this.InitializeComponent();
        }

        private void BackToMainPage(object sender, NavigationViewBackRequestedEventArgs args)
        {
            MainWindow.Current.NavigateToDeviceView();
        }

        private void LanguageSettingRadioGroup_Loaded(object sender, RoutedEventArgs e)
        {
            PreSelectedItem(languageSettingRadioGroup, viewModel.SelectedLanguage);
        }

        private void ThemeSettingRadioGroup_Loaded(object sender, RoutedEventArgs e)
        {
            PreSelectedItem(themeSettingRadioGroup, viewModel.SelectedTheme);
        }

        private static void PreSelectedItem(RadioButtons group, string selectedKey)
        {
            var options = group.Items;
            var option = options[0];
            var selectedItems = options.Where(item => ((RadioButton)item).Tag.ToString() == selectedKey);
            if (selectedItems.Any())
            {
                option = selectedItems.First();
            }
            ((RadioButton)option).IsChecked = true;
        }
    }
}
