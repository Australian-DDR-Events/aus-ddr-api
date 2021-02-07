using System.IO;
using System.Threading.Tasks;

namespace AusDdrApi.Services.FileStorage
{
    public interface IFileStorage
    {
        public Task<string> UploadFileFromStream(Stream stream, string destination);
    }
}