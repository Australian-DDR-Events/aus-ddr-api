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
        public DbSet<Event> Events { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<GradedIngredient> GradedIngredients { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<GradedDancerIngredient> GradedDancerIngredients { get; set; }
        public DbSet<GradedDancerDish> GradedDancerDishes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dancer>()
                .HasIndex(p => p.AuthenticationId)
                .IsUnique();

            modelBuilder.Entity<Score>()
                .Property(s => s.SubmissionTime)
                .HasDefaultValue(DateTime.UtcNow);
            
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
    }
}