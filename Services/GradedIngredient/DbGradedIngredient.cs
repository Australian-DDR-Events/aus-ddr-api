using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using GradedIngredientEntity = AusDdrApi.Entities.GradedIngredient;

namespace AusDdrApi.Services.GradedIngredient
{
    public class DbGradedIngredient : IGradedIngredient
    {
        private readonly DatabaseContext _context;

        public DbGradedIngredient(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GradedIngredientEntity> GetAllForIngredient(Guid ingredientId)
        {
            return _context
                .GradedIngredients
                .AsQueryable()
                .Where(g => g.IngredientId == ingredientId)
                .ToList();
        }

        public GradedIngredientEntity? Get(Guid gradedIngredientId)
        {
            return _context
                .GradedIngredients
                .AsQueryable()
                .FirstOrDefault(g => g.Id == gradedIngredientId);
        }

        public GradedIngredientEntity? GetForScore(Guid ingredientId, int score)
        {
            return _context
                .GradedIngredients
                .AsQueryable()
                .Where(g => g.IngredientId == ingredientId)
                .Where(g => g.RequiredScore <= score)
                .OrderByDescending(ingredient => ingredient.RequiredScore)
                .FirstOrDefault();
        }

        public async Task<GradedIngredientEntity> Add(GradedIngredientEntity gradedIngredient)
        {
            var newGradedIngredient = await _context
                .GradedIngredients
                .AddAsync(gradedIngredient);
            return newGradedIngredient.Entity;
        }

        public GradedIngredientEntity? Update(GradedIngredientEntity gradedIngredient)
        {
            return _context.GradedIngredients.Update(gradedIngredient).Entity;
        }

        public bool Delete(Guid gradedIngredientId)
        {
            var gradedIngredient = Get(gradedIngredientId);
            if (gradedIngredient == null) return false;
            _context.GradedIngredients.Remove(gradedIngredient);
            return true;
        }
    }
}