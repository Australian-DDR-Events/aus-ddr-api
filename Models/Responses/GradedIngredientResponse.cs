using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedIngredientResponse
    {
        public Guid Id { get; set; }
        public string Grade { get; set; }
        public int RequiredScore { get; set; }
        public string Description { get; set; }
        
        public IngredientResponse IngredientResponse { get; set; }
        
        public string ImageUrl { get; set; }
        
        public static GradedIngredientResponse FromEntity(GradedIngredient gradedIngredient) => new GradedIngredientResponse()
        {
            Id = gradedIngredient.Id,
            Grade = gradedIngredient.Grade.ToString("g"),
            RequiredScore = gradedIngredient.RequiredScore,
            Description = gradedIngredient.Description,
            IngredientResponse = IngredientResponse.FromEntity(gradedIngredient.Ingredient),
            ImageUrl = $"/events/summer2021/ingredients/{gradedIngredient.Id}.png",
        };
    }
}
