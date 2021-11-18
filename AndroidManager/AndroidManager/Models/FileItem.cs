using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidManager.Models
{
    public class FileItem
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTimeOffset Time { get; set; }
        public bool IsDirectory { get; set; }

        public static FileItem FromFileStatistics(FileStatistics file)
        {
            return new FileItem
            {
                Name = file.Path,
                Size = file.Size,
                Time = file.Time,
                IsDirectory = file.FileMode.HasFlag(UnixFileMode.Directory)
            };
        }

        public static FileItem UpperFolder => new FileItem
        {
            Name = "..",
            Size = 0,
            Time = DateTimeOffset.Now,
            IsDirectory = true
        };
    }
}
