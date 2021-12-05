using AndroidManager.Models;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AndroidManager.Services
{
    public class DeviceDetailReceiver : MultiLineReceiver
    {
        private readonly DeviceDetail _detail;
        private readonly static Regex PropExtractor = new Regex(@"\[(.+)\]:\s\[(.+)\]");

        public DeviceDetailReceiver()
        {
            _detail = new DeviceDetail();
            TrimLines = true;
        }

        public DeviceDetail Detail { get { return _detail; } }

        protected override void ProcessNewLines(IEnumerable<string> lines)
        {
            foreach(var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                } 
                else
                {
                    ParseProp(line);
                }
            }
        }

        private void ParseProp(string prop)
        {
            var match = PropExtractor.Match(prop);
            if (match.Success 
                && match.Groups.Count == 3 
                && match.Groups[0].Success 
                && match.Groups[1].Success )
            {
                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value)) {
                    return;
                }

                switch (key)
                {
                    case "ro.product.board":
                        _detail.Board = value;
                        break;
                    case "ro.product.brand":
                        _detail.Brand = value;
                        break;
                    case "ro.product.name":
                        _detail.Name = value;
                        break;
                    case "ro.product.model":
                        _detail.Model = value;
                        break;
                    case "ro.product.manufacturer":
                        _detail.Manufacturer = value;
                        break;
                    case "ro.product.locale":
                        _detail.Locale = value;
                        break;
                    case "ro.product.device":
                        _detail.Device = value;
                        break;
                    case "ro.build.date.utc":
                        _detail.BuildDate = DateTimeOffset.FromUnixTimeSeconds(int.Parse(value));
                        break;
                    case "ro.build.fingerprint":
                        _detail.BuildFingerprint = value;
                        break;
                    case "ro.build.id":
                        _detail.BuildId = value;
                        break;
                    case "ro.build.version.sdk":
                        _detail.SdkVersion = value;
                        break;
                    case "ro.build.version.release":
                        _detail.ReleaseVersion = value;
                        break;
                    case "ro.board.platform":
                        _detail.BoardPlatform = value;
                        break;
                }
            }
        }
    }
}
