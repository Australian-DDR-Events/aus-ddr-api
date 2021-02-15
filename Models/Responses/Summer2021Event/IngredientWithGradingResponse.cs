using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class IngredientWithGradingResponse
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SongId { get; set; }
        
        public IEnumerable<GradedIngredientResponse> GradedIngredients { get; set; }

        public static IngredientWithGradingResponse FromEntity(Ingredient ingredient, IEnumerable<GradedIngredient> gradedIngredients)
        {
            if (!gradedIngredients.Any())
            {
                return new IngredientWithGradingResponse();
            }

            var gradedIngredientResponses = gradedIngredients.Select(GradedIngredientResponse.FromEntity);
            return new IngredientWithGradingResponse()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                SongId = ingredient.SongId,
                GradedIngredients = gradedIngredientResponses
            };
        }
    }
}