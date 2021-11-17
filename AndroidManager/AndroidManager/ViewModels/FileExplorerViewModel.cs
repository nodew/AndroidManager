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
        private SyncService _syncService;
        private DeviceData _device;
        private Stack<string> _parentFolder;
        private string _currentFolder;
        private ObservableCollection<FileStatistics> _files;

        public FileExplorerViewModel(AdbClient adbClient, DevicesViewModel devicesViewModel)
        {
            _device = devicesViewModel.CurrentSelectedDeivce;
            _syncService = new SyncService(adbClient, _device);
            _parentFolder = new Stack<string>();
            _files = new ObservableCollection<FileStatistics>();
            _currentFolder = "/";

            NavigateToSubFolderCommand = new RelayCommand<FileStatistics>(NavigateToSubFolder, IsDerectory);
            NavigateToUpperFolderCommand = new RelayCommand(NavigateToUpperFolder, IsRootDirectory);

            LoadChildItems(_currentFolder);
        }

        public ICommand NavigateToSubFolderCommand;
        public ICommand NavigateToUpperFolderCommand;

        public ICommand UploadFileCommand;
        public ICommand DownloadFileCommand;

        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { SetProperty(ref _currentFolder, value); }
        }

        public ObservableCollection<FileStatistics> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }
        }

        private void LoadChildItems(string path)
        {
            Files.Clear();
            var absolutePath = string.Join('/', _parentFolder) + path;
            try
            {
                var files = _syncService.GetDirectoryListing(absolutePath)
                    .OrderBy(file => file.FileMode & UnixFileMode.Directory);
                foreach (var file in files)
                {
                    Files.Add(file);
                }
            } 
            catch (Exception ex)
            {

            }
        }

        private void NavigateToSubFolder(FileStatistics file)
        {
            if (file.Path == ".." && !IsRootDirectory())
            {
                NavigateToUpperFolder();
            }
            else
            {
                _parentFolder.Push(_currentFolder);
                CurrentFolder = file.Path;
                LoadChildItems(file.Path);
            }
        }

        private void NavigateToUpperFolder()
        {
            CurrentFolder = _parentFolder.Pop();
            LoadChildItems(CurrentFolder);
        }

        private bool IsDerectory(FileStatistics file)
        {
            return file.FileMode.HasFlag(UnixFileMode.Directory) && file.Path != ".";
        }

        private bool IsRootDirectory()
        {
            return _currentFolder != "/";
        }
    }
}
