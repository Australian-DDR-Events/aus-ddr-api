using System.IO;
using System.Threading.Tasks;

namespace AusDdrApi.Services.FileStorage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _basePath;
        
        public LocalFileStorage(string basePath)
        {
            _basePath = basePath.TrimEnd('/');
        }
        public async Task<string> UploadFileFromStream(Stream stream, string destination)
        {
            var relativeDestination = destination.TrimStart('/');
            var absoluteDestination = string.Join('/', _basePath, relativeDestination);
            (new FileInfo(absoluteDestination)).Directory.Create();
            await using Stream file = File.Create(absoluteDestination);
            await stream.CopyToAsync(file);
            return string.Join('/', _basePath, destination);
        }
    }
}