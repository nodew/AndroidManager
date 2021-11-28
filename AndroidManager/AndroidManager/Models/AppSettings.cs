using AndroidManager.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AndroidManager.Services
{
    public class AppSettings
    {
        private readonly static string LanguageKey = "Lang";
        private readonly static string ThemeKey = "Theme";
        private readonly static string AdbPathKey = "AdbPath";

        private readonly ApplicationDataContainer _localSettings;

        public AppSettings()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public string Language {
            get => GetStringValue(LanguageKey);
            set => SetStringValue(LanguageKey, value);
        }

        public string AdbPath
        {
            get => GetStringValue(AdbPathKey);
            set => SetStringValue(AdbPathKey, value);
        }

        public string Theme {
            get => GetStringValue(ThemeKey);
            set => SetStringValue(ThemeKey, value);
        }

        private void SetStringValue(string key, string value)
        {
            _localSettings.Values[key] = value;
        }

        private static string GetStringValue(string key)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var value = localSettings.Values[key];
            if (value == null)
            {
                return string.Empty;
            }

            return (string)value;
        }
    }
}
