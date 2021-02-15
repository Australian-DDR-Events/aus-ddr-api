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
        public Guid IngredientId { get; set; }
        
        public static GradedIngredientResponse FromEntity(GradedIngredient gradedIngredient) => new GradedIngredientResponse()
        {
            Id = gradedIngredient.Id,
            Grade = gradedIngredient.Grade.ToString("g"),
            RequiredScore = gradedIngredient.RequiredScore,
            Description = gradedIngredient.Description,
            IngredientId = gradedIngredient.IngredientId,
        };
    }
}
