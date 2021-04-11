using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AusDdrApi.Helpers
{
    public static class Images
    {
        public static async Task<MemoryStream> ImageToPngMemoryStreamFactor(Image image, int maxWidth, int maxHeight)
        {
            var scale = 1.0;
            if (maxWidth > 0) scale = Math.Min(scale, (double)maxWidth / (double)image.Width);
            if (maxHeight > 0) scale = Math.Min(scale, (double)maxHeight / (double)image.Height);
            using var newImage = image.Clone(context => context.Resize((int)(image.Width * scale), (int)(image.Height * scale)));
            
            var memoryStream = new MemoryStream();
            await newImage.SaveAsync(memoryStream, new PngEncoder(), CancellationToken.None);

            return memoryStream;
        }
        public static async Task<MemoryStream> ImageToPngMemoryStream(Image image)
        {
            using var newImage = image.Clone(context => { });

            var memoryStream = new MemoryStream();
            await newImage.SaveAsync(memoryStream, new PngEncoder(), CancellationToken.None);

            return memoryStream;
        }
        public static async Task<MemoryStream> ImageToPngMemoryStream(Image image, int width, int height)
        {
            using var newImage = image.Clone(context => context.Resize(width, height));
            
            var memoryStream = new MemoryStream();
            await newImage.SaveAsync(memoryStream, new PngEncoder(), CancellationToken.None);

            return memoryStream;
        }
    }
}
