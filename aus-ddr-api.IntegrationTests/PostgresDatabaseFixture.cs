
using System;
using AusDdrApi.Persistence;

namespace aus_ddr_api.IntegrationTests
{
    public class PostgresDatabaseFixture : IDisposable
    {
        public DatabaseContext _context { get; }
        
        public PostgresDatabaseFixture()
        {
            _context = Setup.Connect();
            Setup.Migrate(_context);
            Setup.DropAllRows(_context);
        }

        public void Dispose()
        {
            Setup.DropAllRows(_context);
        }
    }
}