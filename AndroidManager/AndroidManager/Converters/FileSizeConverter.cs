using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Converters
{
    public class FileSizeConverter : IValueConverter
    {
        public static int KB = 1024;
        public static int MB = 1024 * KB;
        public static int GB = 1024 * MB;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int size = (int)value;
            if (size >= GB)
            {
                return $"{size / GB} GB";
            }
            
            if (size >= MB)
            {
                return $"{size / MB} MB";
            }

            if (size >= KB)
            {
                return $"{size / KB} KB";
            }

            return "1 KB";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
