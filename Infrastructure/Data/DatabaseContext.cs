using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Dancer> Dancers { get; set; } = default!;
        public DbSet<Score> Scores { get; set; } = default!;
        public DbSet<Song> Songs { get; set; } = default!;
        public DbSet<SongDifficulty> SongDifficulties { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Badge> Badges { get; set; } = default!;
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}