using System;
using AusDdrApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AusDdrApi.Persistence
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Dancer> Dancers { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Song> Songs { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dancer>()
                .HasIndex(p => p.AuthenticationId)
                .IsUnique();

            modelBuilder.Entity<Score>()
                .Property(s => s.SubmissionTime)
                .HasDefaultValue(DateTime.UtcNow);
        }
    }
}