using AndroidManager.Models;
using AndroidManager.Services;
using AndroidManager.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ProcessesView : Page
    {
        private readonly ProcessesViewModel viewModel;

        public ProcessesView()
        {
            viewModel = ServicesProvider.GetService<ProcessesViewModel>();
            this.InitializeComponent();
            WeakReferenceMessenger.Default.Register<ProcessesRefreshed>(this, HandleProcessesRefreshed);
        }

        private void ProcessDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            var selectedColumn = e.Column;
            var selectedTag = selectedColumn.Tag.ToString();
            OrderType orderType = OrderType.None;

            if (selectedColumn.SortDirection == null)
            {
                orderType = OrderType.Ascending;
                selectedColumn.SortDirection = DataGridSortDirection.Ascending;
            }
            else if (selectedColumn.SortDirection == DataGridSortDirection.Ascending)
            {
                orderType = OrderType.Descending;
                selectedColumn.SortDirection = DataGridSortDirection.Descending;
            }
            else
            {
                selectedColumn.SortDirection = null;
            }

            foreach (var dgColumn in processDataGrid.Columns)
            {
                if (dgColumn.Tag.ToString() != selectedTag)
                {
                    dgColumn.SortDirection = null;
                }
            }
            
            viewModel.ReOrderProcessesCommand.Execute(new OrderProcessArg { Order = orderType, OrderBy = selectedTag });
        }

        private void HandleProcessesRefreshed(object recipient, ProcessesRefreshed e)
        {
            CleanupColumnSorting();
        }

        private void CleanupColumnSorting()
        {
            foreach (var dgColumn in processDataGrid.Columns)
            {
                dgColumn.SortDirection = null;
            }
        }
    }
}
