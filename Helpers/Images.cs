using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace AusDdrApi.Helpers
{
    public static class Images
    {
        // TODO: factor-based scaling (restrict size to maximum x/y)
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
