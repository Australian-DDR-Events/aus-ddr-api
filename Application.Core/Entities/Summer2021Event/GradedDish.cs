using System;

namespace Application.Core.Entities
{
    public class GradedDish : BaseEntity
    {
        public Grade Grade { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public Guid DishId { get; set; }
        public Dish? Dish { get; set; }
    }
}