using SharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Models
{
    public class Package
    {
        public string Name { get; set; }

        public bool IsThirdParty { get; set; }
    }
}
