using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Entities.Internal;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class EFDatabaseContext : DbContext
    {
        public EFDatabaseContext(DbContextOptions<EFDatabaseContext> options) : base(options)
        {
        }
        
        /**
         * Uncomment the below and comment the above to create a migration
         * dotnet ef migrations add MyMigrationName --project Infrastructure --verbose
        **/
        // public EFDatabaseContext() : base()
        // {
        // }
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // {
        //     options.UseNpgsql("Username=admin;Password=password;Host=localhost;Port=1235;Database=local");
        // }

        public DbSet<Dancer> Dancers { get; set; } = default!;
        public DbSet<Score> Scores { get; set; } = default!;
        public DbSet<Song> Songs { get; set; } = default!;
        public DbSet<Chart> Charts { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Badge> Badges { get; set; } = default!;

        public DbSet<Reward> Rewards { get; set; } = default!;
        public DbSet<RewardQuality> RewardQualities { get; set; } = default!;
        public DbSet<RewardTrigger> RewardTriggers { get; set; } = default!;

        public DbSet<Connection> Connections { get; set; } = default!;

        public DbSet<Session> Sessions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dancer>()
                .HasIndex(p => p.AuthenticationId)
                .IsUnique();

            modelBuilder.Entity<Score>()
                .Property(s => s.SubmissionTime)
                .HasDefaultValue(DateTime.UtcNow);
            
            modelBuilder
                .Entity<Chart>()
                .Property(i => i.PlayMode)
                .HasConversion(
                    ig => ig.ToString(),
                    ig => (PlayMode) Enum.Parse(typeof(PlayMode), ig));
            
            modelBuilder
                .Entity<Chart>()
                .Property(i => i.Difficulty)
                .HasConversion(
                    ig => ig.ToString(),
                    ig => (Difficulty) Enum.Parse(typeof(Difficulty), ig));
            
            modelBuilder
                .Entity<GradedIngredient>()
                .Property(i => i.Grade)
                .HasConversion(
                    ig => ig.ToString(),
                    ig => (Grade) Enum.Parse(typeof(Grade), ig));
            
            modelBuilder
                .Entity<GradedDish>()
                .Property(i => i.Grade)
                .HasConversion(
                    ig => ig.ToString(),
                    ig => (Grade) Enum.Parse(typeof(Grade), ig));
        }

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