using System.Threading.Tasks;

namespace AusDdrApi.Services.CoreData
{
    public interface ICoreData
    {
        public Task SaveChanges();
    }
}
