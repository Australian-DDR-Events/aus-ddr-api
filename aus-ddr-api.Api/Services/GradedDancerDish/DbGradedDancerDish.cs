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
                .Include(g => g.GradedDish)
                .Include(g => g.Scores)
                .AsSplitQuery()
                .Where(g => g.DancerId == dancerId)
                .ToList();
        }

        public IEnumerable<GradedDancerDishEntity> GetTopForDancer(Guid dancerId)
        {
            return _context
                .GradedDancerDishes
                .Where(z => z.DancerId == dancerId)
                .Include(x => x.GradedDish)
                .ThenInclude(d => d!.Dish)
                .Include(x => x.Scores)
                .ThenInclude(s => s!.Song)
                .AsSplitQuery()
                .AsEnumerable()
                .GroupBy(x => x.GradedDish!.DishId)
                .Select(x => x.Aggregate(
                    (l, r) => 
                        l.Scores.Sum(s => s.Value) > l.Scores.Sum(s => s.Value) ? l : r));
        }

        public GradedDancerDishEntity? GetDishForDancer(Guid dishId, Guid dancerId)
        {
            return _context
                .GradedDancerDishes
                .Include(g => g.GradedDish)
                .Include(g => g.Scores)
                .AsSplitQuery()
                .Where(g => g.DancerId == dancerId)
                .Where(g => g.GradedDish!.DishId == dishId)
                .OrderByDescending(g => g.Scores.Sum(s => s.Value))
                .FirstOrDefault();
        }

        public GradedDancerDishEntity? Get(Guid gradedDancerDishId)
        {
            return _context
                .GradedDancerDishes
                .Include(g => g.GradedDish)
                .Include(g => g.Scores)
                .AsSplitQuery()
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