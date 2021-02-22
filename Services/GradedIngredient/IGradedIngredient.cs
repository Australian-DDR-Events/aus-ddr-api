using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GradedIngredientEntity = AusDdrApi.Entities.GradedIngredient;

namespace AusDdrApi.Services.GradedIngredient
{
    public interface IGradedIngredient
    {
        public IEnumerable<GradedIngredientEntity> GetAllForIngredient(Guid ingredientId);
        public GradedIngredientEntity? Get(Guid gradedIngredientId);
        public GradedIngredientEntity? GetForScore(Guid ingredientId, int score);

        public Task<GradedIngredientEntity> Add(GradedIngredientEntity gradedIngredient);
        public GradedIngredientEntity? Update(GradedIngredientEntity gradedIngredient);
        public bool Delete(Guid gradedIngredientId);
    }
}