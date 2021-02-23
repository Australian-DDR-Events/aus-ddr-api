using System.Threading.Tasks;
using AusDdrApi.Persistence;

namespace AusDdrApi.Services.CoreData
{
    public class DbCoreData : ICoreData
    {
        private DatabaseContext _context;
        
        public DbCoreData(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}