
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using Microsoft.EntityFrameworkCore;
using GradedDancerIngredientEntity = AusDdrApi.Entities.GradedDancerIngredient;
namespace AusDdrApi.Services.GradedDancerIngredient
{
    public class DbGradedDancerIngredient : IGradedDancerIngredient
    {
        private readonly DatabaseContext _context;

        public DbGradedDancerIngredient(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GradedDancerIngredientEntity> GetAllForDancer(Guid dancerId)
        {
            return _context
                .GradedDancerIngredients
                .AsQueryable()
                .Where(g => g.DancerId == dancerId)
                .ToList();
        }

        public IEnumerable<GradedDancerIngredientEntity> GetTopForDancer(Guid dancerId)
        {
            // TODO: this performs grouping locally rather than on the database. This can
            // result in poor performance. This will need to be reworked to instead run 
            // on the database.
            return _context
                .GradedDancerIngredients
                .Include(s => s.Score)
                .AsEnumerable()
                .GroupBy(ingredient => ingredient.Score!.SongId)
                .Select(i => i
                    .OrderByDescending(g => g.Score!.Value)
                    .First())
                .ToList();
        }

        public IEnumerable<GradedDancerIngredientEntity> GetAllForIngredient(Guid ingredientId)
        {
            return _context
                .GradedDancerIngredients
                .AsQueryable()
                .Include(g => g.GradedIngredient)
                .Where(g => g.GradedIngredient!.IngredientId == ingredientId)
                .ToList();
        }

        public GradedDancerIngredientEntity? Get(Guid gradedDancerIngredientId)
        {
            return _context
                .GradedDancerIngredients
                .AsQueryable()
                .SingleOrDefault(g => g.Id == gradedDancerIngredientId);
        }

        public GradedDancerIngredientEntity? GetIngredientForDancer(Guid ingredientId, Guid dancerId)
        {
            return _context
                .GradedDancerIngredients
                .Include(g => g.GradedIngredient)
                .Include(g => g.Score)
                .AsQueryable()
                .Where(g => g.DancerId == dancerId)
                .Where(g => g.GradedIngredient!.IngredientId == ingredientId)
                .OrderByDescending(g => g.Score!.Value)
                .FirstOrDefault();
        }

        public IEnumerable<GradedDancerIngredientEntity> GetIngredientsForDancer(IEnumerable<Guid> ingredientIds,
            Guid dancerId)
        {
            var gradedDancerIngredients = _context
                .GradedDancerIngredients
                .Include(g => g.GradedIngredient)
                .Include(g => g.Score)
                .AsQueryable();

            // TODO: this performs grouping locally rather than on the database. This can
            // result in poor performance. This will need to be reworked to instead run 
            // on the database.
            return gradedDancerIngredients
                .Where(g => g.DancerId == dancerId)
                .Where(g => ingredientIds.Contains(g.GradedIngredient!.IngredientId))
                .AsEnumerable()
                .GroupBy(g => g.GradedIngredient!.IngredientId)
                .Select(g => g
                    .OrderByDescending(i => i.Score!.Value)
                    .First())
                .ToList();
        }

        public async Task<GradedDancerIngredientEntity> Add(GradedDancerIngredientEntity gradedDancerIngredient)
        {
            var newGradedDancerIngredient = await _context
                .GradedDancerIngredients
                .AddAsync(gradedDancerIngredient);
            return newGradedDancerIngredient.Entity;
        }

        public GradedDancerIngredientEntity? Update(GradedDancerIngredientEntity gradedDancerIngredient)
        {
            return _context
                .GradedDancerIngredients
                .Update(gradedDancerIngredient)
                .Entity;
        }

        public bool Delete(Guid gradedDancerIngredientId)
        {
            var gradedDancerIngredient = Get(gradedDancerIngredientId);
            if (gradedDancerIngredient != null)
            {
                _context.GradedDancerIngredients.Remove(gradedDancerIngredient);
                return true;
            }

            return false;
        }
    }
}