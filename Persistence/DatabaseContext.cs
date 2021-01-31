using AusDdrApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.Persistence
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Dancer> Dancers { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dancer>()
                .HasIndex(p => p.AuthenticationId)
                .IsUnique();
        }
    }
}