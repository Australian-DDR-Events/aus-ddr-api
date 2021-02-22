using System;

namespace AusDdrApi.Entities
{
    public class GradedIngredient
    {
        public Guid Id { get; set; }
        public Grade Grade { get; set; }
        public int RequiredScore { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public Guid IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
