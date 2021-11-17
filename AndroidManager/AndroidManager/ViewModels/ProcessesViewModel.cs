using Microsoft.Toolkit.Mvvm.Input;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AndroidManager.ViewModels
{
    public class ProcessesViewModel : ViewModelBase
    {
        private AdbClient _adbClient;
        private DeviceData _device;

        private ObservableCollection<AndroidProcess> _processes;

        public ProcessesViewModel(AdbClient adbClient, DevicesViewModel devicesViewModel)
        {
            _adbClient = adbClient;
            _device = devicesViewModel.CurrentSelectedDeivce;
            _processes = new ObservableCollection<AndroidProcess>();

            RefreshProcessesCommand = new RelayCommand(RefreshProcesses);

            LoadProcesses();
        }

        public ICommand RefreshProcessesCommand;

        public ObservableCollection<AndroidProcess> Processes
        {
            get { return _processes; }
            set { SetProperty(ref _processes, value); }
        }

        private void LoadProcesses()
        {
            Processes.Clear();
            foreach (var process in _adbClient.ListProcesses(_device))
            {
                Processes.Add(process);
            }
        }

        private void RefreshProcesses()
        {
            LoadProcesses();
        }
    }
}
