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
    public sealed partial class FileExplorerView : Page
    {

        private FileExplorerViewModel viewModel;

        public FileExplorerView()
        {
            viewModel = App.Current.Services.GetService<FileExplorerViewModel>();
            this.InitializeComponent();
        }

        private void FileListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            viewModel.NavigateToSubFolderCommand.Execute(e.ClickedItem);
        }
    }
}
