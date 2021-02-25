using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
using GradedDancerDishEntity = AusDdrApi.Entities.GradedDancerDish;

namespace AusDdrApi.Services.GradedDancerDish
{
    public class DbGradedDancerDish : IGradedDancerDish
    {
        private readonly DatabaseContext _context;

        public DbGradedDancerDish(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GradedDancerDishEntity> GetAllForDancer(Guid dancerId)
        {
            return _context
                .GradedDancerDishes
                .AsQueryable()
                .Where(g => g.DancerId == dancerId)
                .AsEnumerable();
        }

        public IEnumerable<GradedDancerDishEntity> GetTopForDancer(Guid dancerId)
        {
            return _context
                .GradedDancerDishes
                .Include(g => g.GradedDish)
                .Include(g => g.Scores)
                .AsQueryable()
                .GroupBy(g => g.GradedDish!.DishId)
                .Select(i => i
                    .OrderByDescending(g => g.Scores.Aggregate(0, (v, s) => v + s.Value))
                    .Single())
                .AsEnumerable();
        }

        public GradedDancerDishEntity? GetDishForDancer(Guid dishId, Guid dancerId)
        {
            return _context
                .GradedDancerDishes
                .Include(g => g.GradedDish)
                .Include(g => g.Scores)
                .AsQueryable()
                .Where(g => g.DancerId == dancerId)
                .Where(g => g.GradedDish!.DishId == dishId)
                .OrderByDescending(g => g.Scores.Aggregate(0, (v, s) => v + s.Value))
                .SingleOrDefault();
        }

        public GradedDancerDishEntity? Get(Guid gradedDancerDishId)
        {
            return _context
                .GradedDancerDishes
                .AsQueryable()
                .SingleOrDefault(g => g.Id == gradedDancerDishId);
        }

        public async Task<GradedDancerDishEntity?> Add(GradedDancerDishEntity gradedDancerDish)
        {
            var newGradedDancerDish = await _context
                .AddAsync(gradedDancerDish);
            return newGradedDancerDish.Entity;
        }
    }
}