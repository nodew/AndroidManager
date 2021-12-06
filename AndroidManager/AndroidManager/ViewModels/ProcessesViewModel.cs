using AndroidManager.Models;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
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
        private IAdbClient _adbClient;
        private DeviceData _device;

        private ObservableCollection<AndroidProcess> _processes;
        private List<AndroidProcess> _originalOrderedProcesses;

        public ProcessesViewModel(IAdbClient adbClient, MainWindowViewModel mainWindowViewModel)
        {
            _adbClient = adbClient;
            _device = mainWindowViewModel.CurrentSelectedDevice;
            _processes = new ObservableCollection<AndroidProcess>();
            _originalOrderedProcesses = new List<AndroidProcess>();

            RefreshProcessesCommand = new RelayCommand(RefreshProcesses);
            ReOrderProcessesCommand = new RelayCommand<OrderProcessArg>(ReOrderProcesses);

            LoadProcesses();
        }

        public ICommand RefreshProcessesCommand;
        public ICommand ReOrderProcessesCommand;

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

            _originalOrderedProcesses = Processes.ToList();
            WeakReferenceMessenger.Default.Send(new ProcessesRefreshed());
        }

        private void RefreshProcesses()
        {
            LoadProcesses();
        }

        private void ReOrderProcesses(OrderProcessArg arg)
        {
            var propertyInfo = typeof(AndroidProcess).GetProperty(arg.OrderBy);
            IEnumerable<AndroidProcess> items;
            if (propertyInfo == null)
            {
                items = _originalOrderedProcesses;
            }
            else
            {
                if (arg.Order == OrderType.Ascending)
                {
                    items = _originalOrderedProcesses.OrderBy(x => propertyInfo.GetValue(x, null));
                }
                else if (arg.Order == OrderType.Descending)
                {
                    items = _originalOrderedProcesses.OrderByDescending(x => propertyInfo.GetValue(x, null));
                }
                else
                {
                    items = _originalOrderedProcesses;
                }
            }

            Processes.Clear();
            foreach (var item in items)
            {
                Processes.Add(item);
            }
        }
    }
}
