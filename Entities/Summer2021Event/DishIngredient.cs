using System;

namespace AusDdrApi.Entities
{
    public class DishIngredient
    {
        public Guid Id { get; set; }
        
        public Guid DishId { get; set; }
        public Dish Dish { get; set; }
        
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}