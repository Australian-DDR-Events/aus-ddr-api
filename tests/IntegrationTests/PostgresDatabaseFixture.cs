using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests
{
    public class PostgresDatabaseFixture : IDisposable
    {
        public EFDatabaseContext _context { get; }
        
        public PostgresDatabaseFixture()
        {
            _context = Setup.Connect();
            Setup.Migrate(_context);
            Setup.DropAllRows(_context);
        }

        public void Dispose()
        {
            Setup.DropAllRows(_context);
            Setup.Destroy(_context);
            _context.Database.CloseConnection();
        }
    }
}