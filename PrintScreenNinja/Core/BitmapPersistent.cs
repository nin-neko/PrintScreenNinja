using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace PrintScreenNinja.Core
{
    sealed class BitmapPersistent
    {
        private readonly object _locker = new();

        public string Directory { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        //public string FileNamePattern { get; set; } = "MMdd_HH-mm-ss_$id";

        public string Extension { get; set; } = ".bmp";

        public bool SetDirectory(string value)
        {
            lock (_locker)
            {
                var oldValue = this.Directory;
                if (oldValue != value && System.IO.Directory.Exists(value))
                {
                    this.Directory = value;
                    return true;
                }
            }

            return false;
        }

        public async Task SaveAsync(byte[] bitmapBytes, CancellationToken cancellationToken = default)
        {
            var withDate = DateTimeOffset.Now.ToString("MMdd_HHmmss");
            var withId = Guid.NewGuid().ToString().Replace("-", "_");
            var name = $"{withDate}_{withId}{this.Extension}";

            var filePath = "";
            lock (_locker)
            {
                filePath = Path.Combine(this.Directory, name);
            }

            await File.WriteAllBytesAsync(filePath, bitmapBytes, cancellationToken);
        }
    }
}
