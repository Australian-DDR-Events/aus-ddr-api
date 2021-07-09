using System.IO;
using System.Threading.Tasks;

namespace Application.Core.Interfaces
{
    public interface IFileStorage
    {
        public Task<string> UploadFileFromStream(Stream stream, string destination);
        public Task DeleteFile(string destination);
    }
}