using System;

namespace Application.Core.Entities
{
    public class GradedIngredient : BaseEntity
    {
        public Grade Grade { get; set; }
        public int RequiredScore { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public Guid IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
