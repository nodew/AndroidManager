using AndroidManager.Models;
using AndroidManager.Services;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AndroidManager.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private AppSettings _appSettings;
        private string _selectedLanguage;
        private string _selectedTheme;
        private string _adbPath;

        public ICommand UpdateLanguageOptionCommand;
        public ICommand UpdateThemeOptionCommand;
        public ICommand SaveChangesCommand;

        public SettingsViewModel(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _adbPath = appSettings.AdbPath;
            _selectedLanguage = appSettings.Language;
            _selectedTheme = appSettings.Theme;

            UpdateLanguageOptionCommand = new RelayCommand<string>(UpdateLanguageOption, CanUpdateOption);
            UpdateThemeOptionCommand = new RelayCommand<string>(UpdateThemeOption, CanUpdateOption);
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        public string SelectedLanguage {
            get { return _selectedLanguage; }
            set { SetProperty(ref _selectedLanguage, value); }
        }

        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set { SetProperty(ref _selectedTheme, value); }
        }

        public string AdbPath
        {
            get { return _adbPath; }
            set { SetProperty(ref _adbPath, value); }
        }

        private void UpdateLanguageOption(string lang)
        {
            if (lang == "default")
            {
                lang = string.Empty;
            }
            SelectedLanguage = lang;
        }

        private void UpdateThemeOption(string theme)
        {
            if (theme == "default")
            {
                theme = string.Empty;
            }
            SelectedTheme = theme;
            MainWindow.Current.ApplyThemeSetting(theme);
        }

        private bool CanUpdateOption(string option)
        {
            return !string.IsNullOrWhiteSpace(option);
        }

        private void SaveChanges()
        {
            _appSettings.Language = SelectedLanguage;
            _appSettings.Theme = SelectedTheme;
            _appSettings.AdbPath = AdbPath;

            MainWindow.Current.ApplyLanguageSetting(SelectedLanguage);
        }
    }
}
