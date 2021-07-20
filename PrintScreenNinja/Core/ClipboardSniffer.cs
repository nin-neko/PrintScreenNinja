using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;


namespace PrintScreenNinja.Core
{
    static class ClipboardSniffer
    {
        static ClipboardSniffer()
        {
            BitmapBytesChanged = Observable.FromEventPattern<object>(h => Clipboard.ContentChanged += h, h => Clipboard.ContentChanged -= h)
                .Where(_ => HasBitmap())
                .SelectMany(_ => ConvertToBitmapBytesAsync(Clipboard.GetContent(), CancellationToken.None).ToObservable());
        }

        private static async Task<byte[]> ConvertToBitmapBytesAsync(DataPackageView dataPackageView, CancellationToken cancellationToken)
        {
            var bitmap = await dataPackageView.GetBitmapAsync();
            using var randomAccessStream = await bitmap.OpenReadAsync();
            using var stream = randomAccessStream.AsStreamForRead();
            using var ms = new MemoryStream(stream.Length > int.MaxValue ? int.MaxValue : (int)stream.Length);
            await stream.CopyToAsync(ms, cancellationToken);
            return ms.ToArray();
        }

        private static bool HasBitmap()
        {
            var dataPackageView = Clipboard.GetContent();
            return dataPackageView.Contains(StandardDataFormats.Bitmap);
        }

        public static IObservable<byte[]> BitmapBytesChanged { get; }
    }
}
