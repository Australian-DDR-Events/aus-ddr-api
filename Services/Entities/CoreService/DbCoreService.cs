using System.Threading.Tasks;
using AusDdrApi.Persistence;

namespace AusDdrApi.Services.Entities.CoreService
{
    public class DbCoreService : ICoreService
    {
        private DatabaseContext _context;
        
        public DbCoreService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}