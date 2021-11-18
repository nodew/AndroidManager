using AndroidManager.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
            FileItem file = (FileItem)item;
            if (file == null)
            {
                return null;
            }

            if (file.IsDirectory)
            {
                return FolderTemplate;
            }

            return FileTemplate;
        }
    }
}
