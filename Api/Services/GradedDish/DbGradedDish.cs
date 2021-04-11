using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Entities;
using AusDdrApi.Persistence;
using GradedDishEntity = AusDdrApi.Entities.GradedDish;

namespace AusDdrApi.Services.GradedDish
{
    public class DbGradedDish : IGradedDish
    {
        private readonly DatabaseContext _context;

        public DbGradedDish(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GradedDishEntity> GetAllForDish(Guid dishId)
        {
            return _context
                .GradedDishes
                .AsQueryable()
                .Where(g => g.DishId == dishId)
                .ToList();
        }

        public GradedDishEntity? Get(Guid gradedDishId)
        {
            return _context
                .GradedDishes
                .AsQueryable()
                .SingleOrDefault(g => g.Id == gradedDishId);
        }

        public GradedDishEntity? GetForDishIdAndGrade(Guid dishId, Grade grade)
        {
            return _context
                .GradedDishes
                .AsQueryable()
                .Where(g => g.DishId == dishId)
                .SingleOrDefault(g => g.Grade == grade);
        }

        public async Task<GradedDishEntity> Add(GradedDishEntity gradedDish)
        {
            var newGradedDish = await _context
                .GradedDishes
                .AddAsync(gradedDish);
            return newGradedDish.Entity;
        }

        public GradedDishEntity? Update(GradedDishEntity gradedDish)
        {
            return _context.GradedDishes.Update(gradedDish).Entity;
        }

        public bool Delete(Guid gradedDishId)
        {
            var gradedDish = Get(gradedDishId);
            if (gradedDish == null) return false;
            _context.GradedDishes.Remove(gradedDish);
            return true;

        }
    }
}