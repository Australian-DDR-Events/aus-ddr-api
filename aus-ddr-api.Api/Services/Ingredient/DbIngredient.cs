using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Persistence;
using IngredientEntity = AusDdrApi.Entities.Ingredient;

namespace AusDdrApi.Services.Ingredient
{
    public class DbIngredient : IIngredient
    {
        private readonly DatabaseContext _context;

        public DbIngredient(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<IngredientEntity> GetAll()
        {
            return _context.Ingredients.AsQueryable().ToArray();
        }

        public IngredientEntity? Get(Guid ingredientId)
        {
            return _context.Ingredients.AsQueryable().SingleOrDefault(i => i.Id == ingredientId);
        }
        
        public IEnumerable<IngredientEntity> Get(IEnumerable<Guid> ingredientIds)
        {
            return _context.Ingredients.AsQueryable().Where(i => ingredientIds.Contains(i.Id));
        }

        public async Task<IngredientEntity> Add(IngredientEntity ingredient)
        {
            var ingredientEntity = await _context.Ingredients.AddAsync(ingredient);
            return ingredientEntity.Entity;
        }

        public IngredientEntity Update(IngredientEntity ingredient)
        {
            return _context.Ingredients.Update(ingredient).Entity;
        }

        public bool Delete(Guid ingredientId)
        {
            var ingredient = Get(ingredientId);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                return true;
            }

            return false;
        }
    }
}