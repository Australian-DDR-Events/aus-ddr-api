using System.IO;
using System.Threading.Tasks;

namespace AusDdrApi.Services.FileStorage
{
    public interface IFileStorage
    {
        public Task<string> UploadFileFromStream(MemoryStream stream, string destination);
    }
}