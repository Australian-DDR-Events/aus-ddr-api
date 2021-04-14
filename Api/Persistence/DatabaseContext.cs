using System;
using AusDdrApi.Entities;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace AusDdrApi.Persistence
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<Dancer> Dancers { get; set; } = default!;
        public DbSet<Score> Scores { get; set; } = default!;
        public DbSet<Song> Songs { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Badge> Badges { get; set; } = default!;
        public DbSet<Ingredient> Ingredients { get; set; } = default!;
        public DbSet<GradedIngredient> GradedIngredients { get; set; } = default!;
        public DbSet<Dish> Dishes { get; set; } = default!;
        public DbSet<DishSong> DishSongs { get; set; } = default!;
        public DbSet<GradedDancerIngredient> GradedDancerIngredients { get; set; } = default!;
        
        public DbSet<GradedDish> GradedDishes { get; set; } = default!;
        public DbSet<GradedDancerDish> GradedDancerDishes { get; set; } = default!;
        
        public DbSet<BadgeThreshold> BadgeThresholds { get; set; } = default!;
        
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
#nullable restore
