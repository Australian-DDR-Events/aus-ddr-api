using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GradedDancerIngredientEntity = AusDdrApi.Entities.GradedDancerIngredient;

namespace AusDdrApi.Services.GradedDancerIngredient
{
    public interface IGradedDancerIngredient
    {
        public IEnumerable<GradedDancerIngredientEntity> GetAllForDancer(Guid dancerId);
        public IEnumerable<GradedDancerIngredientEntity> GetTopForDancer(Guid dancerId);
        public IEnumerable<GradedDancerIngredientEntity> GetAllForIngredient(Guid ingredientId);
        public GradedDancerIngredientEntity? GetIngredientForDancer(Guid ingredientId, Guid dancerId);

        public IEnumerable<GradedDancerIngredientEntity> GetIngredientsForDancer(IEnumerable<Guid> ingredientIds,
            Guid dancerId);
        public GradedDancerIngredientEntity? Get(Guid gradedDancerIngredientId);

        public Task<GradedDancerIngredientEntity> Add(GradedDancerIngredientEntity gradedIngredient);
        public GradedDancerIngredientEntity? Update(GradedDancerIngredientEntity gradedDancerIngredient);
        public bool Delete(Guid gradedDancerIngredientId);
    }
}