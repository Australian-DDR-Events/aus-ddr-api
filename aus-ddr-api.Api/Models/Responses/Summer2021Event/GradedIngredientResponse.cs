using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedIngredientResponse
    {
        public Guid Id { get; set; }
        public string Grade { get; set; } = string.Empty;
        public int RequiredScore { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image32 { get; set; } = string.Empty;
        public string Image64 { get; set; } = string.Empty;
        public string Image128 { get; set; } = string.Empty;
        public string Image256 { get; set; } = string.Empty;
        
        public static GradedIngredientResponse FromEntity(GradedIngredient gradedIngredient) => new GradedIngredientResponse()
        {
            Id = gradedIngredient.Id,
            Grade = gradedIngredient.Grade.ToString("g"),
            RequiredScore = gradedIngredient.RequiredScore,
            Name = gradedIngredient.Ingredient?.Name ?? string.Empty,
            Description = gradedIngredient.Description,
            Image32 = $"/summer2021/gradedingredients/{gradedIngredient.Id}.32.png",
            Image64 = $"/summer2021/gradedingredients/{gradedIngredient.Id}.64.png",
            Image128 = $"/summer2021/gradedingredients/{gradedIngredient.Id}.128.png",
            Image256 = $"/summer2021/gradedingredients/{gradedIngredient.Id}.256.png"
        };
    }
}
