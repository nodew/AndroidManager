using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Services
{
    public class PackageReceiver : MultiLineReceiver
    {
        private const string PackagePrefix = "package:";

        private readonly List<string> _packages;

        public PackageReceiver()
        {
            _packages = new List<string>();
            TrimLines = true;
        }

        public List<string> Packages { get { return _packages; } }
            
        protected override void ProcessNewLines(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && line.StartsWith(PackagePrefix)) 
                { 
                    _packages.Add(line[PackagePrefix.Length..]);
                }; 
            }
        }
    }
}
