using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Views
{
    public class FileItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }
        public DataTemplate FolderTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            FileStatistics file = (FileStatistics)item;
            if (file == null)
            {
                return null;
            }

            if (file.FileMode.HasFlag(UnixFileMode.Directory))
            {
                return FolderTemplate;
            }

            return FileTemplate;
        }
    }
}
