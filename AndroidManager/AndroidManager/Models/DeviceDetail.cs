using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Models
{
    public class DeviceDetail
    {
        public string Name { get; set; }

        public string Model { get; set; }

        public string Board { get; set; }

        public string Brand { get; set; } 
        
        public string Device { get; set; }

        public string Manufacturer { get; set; }

        public string Locale { get; set; } 

        public string SdkVersion { get; set; }

        public string ReleaseVersion { get; set; }

        public DateTimeOffset BuildDate { get; set; }

        public string BuildFingerprint { get; set; }

        public string BuildId { get; set; }

        public string BoardPlatform { get; set; }
    }
}
