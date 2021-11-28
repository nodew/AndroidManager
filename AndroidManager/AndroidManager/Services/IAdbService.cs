using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Services
{
    public interface IAdbService
    {
        public string GetAdbExePath();

        public void StartAdbServer();

        public void StartAdbServer(string adbPath);
    }
}
