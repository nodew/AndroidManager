using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Models
{
    public class Device : DeviceData
    {
        public string ReleaseVersion { get; set; }

        public string SdkVersion { get; set; }

        public string Brand { get; set; }

        public string DisplayName
        {
            get {
                if (State == DeviceState.Online)
                {
                    return string.Format("{0} {1} (Android {2} - API {3})", Brand, Model, ReleaseVersion, SdkVersion);
                }

                return Model;
            }
        }

        public static Device FromDeviceData(DeviceData data)
        {
            var device = new Device
            {
                Serial = data.Serial,
                State = data.State,
                Model = data.Model,
                Product = data.Product,
                Name = data.Name,
                Features = data.Features,
                Usb = data.Usb,
                TransportId = data.TransportId,
                Message = data.Message
            };
            return device;
        }
    }
}
