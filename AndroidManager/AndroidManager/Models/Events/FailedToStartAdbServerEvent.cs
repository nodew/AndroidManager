using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Models
{
    public class FailedToStartAdbServerEvent
    {
        public FailedToStartAdbServerEvent(string message)
        {
            ErrorMessage = message;
        }

        public string ErrorMessage { get; set; }
    }
}
