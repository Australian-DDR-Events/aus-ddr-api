using System;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string Image32 => $"/summer2021/gradedingredients/{Id}.32.png";
        [NotMapped]
        public string Image64 => $"/summer2021/gradedingredients/{Id}.64.png";
        [NotMapped]
        public string Image128 => $"/summer2021/gradedingredients/{Id}.128.png";
        [NotMapped]
        public string Image256 => $"/summer2021/gradedingredients/{Id}.256.png";
    }
}
