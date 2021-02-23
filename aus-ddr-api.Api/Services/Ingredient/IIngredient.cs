using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IngredientEntity = AusDdrApi.Entities.Ingredient;

namespace AusDdrApi.Services.Ingredient
{
    public interface IIngredient
    {
        public IEnumerable<IngredientEntity> GetAll();
        public IngredientEntity? Get(Guid ingredientId);
        public IEnumerable<IngredientEntity> Get(IEnumerable<Guid> ingredientIds);

        public Task<IngredientEntity> Add(IngredientEntity ingredient);
        public IngredientEntity Update(IngredientEntity ingredient);
        public bool Delete(Guid ingredientId);
    }
}