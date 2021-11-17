using Microsoft.UI.Xaml.Data;
using SharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Converters
{
    public class AndroidStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            AndroidProcessState state = (AndroidProcessState)value;
            return state switch
            {
                AndroidProcessState.D => "Uninterruptible sleep",
                AndroidProcessState.R => "Running",
                AndroidProcessState.S => "Interruptible sleep",
                AndroidProcessState.T => "Stopped",
                AndroidProcessState.W => "Paging",
                AndroidProcessState.X => "Dead",
                AndroidProcessState.Z => "Defunct",
                AndroidProcessState.K => "Wakekill",
                AndroidProcessState.P => "Parked",
                _ => "Unknown",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
