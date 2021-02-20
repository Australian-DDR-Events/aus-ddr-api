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
        public static async Task<MemoryStream> FormFileToPngMemoryStream(IFormFile file)
        {
            using var image = await Image.LoadAsync(file.OpenReadStream());

            await using var memoryStream = new MemoryStream();
            await image.SaveAsync(memoryStream, new PngEncoder(), CancellationToken.None);

            return memoryStream;
        }
        public static async Task<MemoryStream> FormFileToPngMemoryStream(IFormFile file, int width, int height)
        {
            using var image = await Image.LoadAsync(file.OpenReadStream());
            
            image.Mutate(x => x.Resize(width, height));

            await using var memoryStream = new MemoryStream();
            await image.SaveAsync(memoryStream, new PngEncoder(), CancellationToken.None);

            return memoryStream;
        }
    }
}
