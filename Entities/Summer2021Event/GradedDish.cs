using System;

namespace AusDdrApi.Entities
{
    public class GradedDish
    {
        public Guid Id { get; set; }
        public Grade Grade { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public Guid DishId { get; set; }
        public Dish? Dish { get; set; }
    }
}