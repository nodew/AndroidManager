using AndroidManager.Models;
using Microsoft.Toolkit.Mvvm.Input;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AndroidManager.ViewModels
{
    public class FileExplorerViewModel : ViewModelBase
    {
        private IAdbClient _adbClient;
        private DeviceData _device;
        private Stack<string> _parentFolder;
        private string _currentFolder;
        private ObservableCollection<FileItem> _files;

        public FileExplorerViewModel(IAdbClient adbClient, DevicesViewModel devicesViewModel)
        {
            _adbClient = adbClient;
            _device = devicesViewModel.CurrentSelectedDevice;
            _parentFolder = new Stack<string>();
            _files = new ObservableCollection<FileItem>();
            _currentFolder = "/";

            NavigateToFolderCommand = new RelayCommand<FileItem>(NavigateToFolder, IsDirectory);

            LoadChildItems(_currentFolder);
        }

        public ICommand NavigateToFolderCommand;
        public ICommand UploadFileCommand;
        public ICommand DownloadFileCommand;

        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { SetProperty(ref _currentFolder, value); }
        }

        public ObservableCollection<FileItem> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }
        }

        private void LoadChildItems(string path)
        {
            Files.Clear();
            var absolutePath = string.Join('/', _parentFolder) + path;
            using SyncService syncService = new SyncService(_adbClient, _device);
            try
            {
                var items = syncService.GetDirectoryListing(absolutePath)
                    .Where(file => !((file.Path == ".") || (file.Path == "..")))
                    .Select(file => FileItem.FromFileStatistics(file));

                var folders = items.Where(file => file.IsDirectory)
                    .OrderBy(file => file.Name);
                
                var files = items.Where(file => !file.IsDirectory)
                    .OrderBy(file => file.Name);

                if (!IsRootDirectory()) {
                    Files.Add(new FileItem { Name = "..", IsDirectory = true });
                }
                
                foreach (var folder in folders)
                {
                    Files.Add(folder);
                }

                foreach (var file in files)
                {
                    Files.Add(file);
                }
            }
            catch (Exception)
            {

            }
        }

        private void NavigateToFolder(FileItem file)
        {
            if (file.Name == "..")
            {
                if (!IsRootDirectory()) 
                {
                    NavigateToUpperFolder();
                }
            }
            else
            {
                _parentFolder.Push(_currentFolder);
                CurrentFolder = file.Name;
                LoadChildItems(file.Name);
            }
        }

        private void NavigateToUpperFolder()
        {
            CurrentFolder = _parentFolder.Pop();
            LoadChildItems(CurrentFolder);
        }

        private bool IsDirectory(FileItem file)
        {
            return file.IsDirectory;
        }

        private bool IsRootDirectory()
        {
            return _currentFolder == "/";
        }
    }
}
