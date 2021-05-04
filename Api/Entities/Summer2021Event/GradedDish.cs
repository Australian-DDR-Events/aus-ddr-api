using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AusDdrApi.Entities
{
    public class GradedDish
    {
        public Guid Id { get; set; }
        public Grade Grade { get; set; }
        public string Description { get; set; } = string.Empty;
        
        public Guid DishId { get; set; }
        public Dish? Dish { get; set; }

        [NotMapped]
        public string Image32 => $"/summer2021/gradeddishes/{Id}.32.png";
        [NotMapped]
        public string Image64 => $"/summer2021/gradeddishes/{Id}.64.png";
        [NotMapped]
        public string Image128 => $"/summer2021/gradeddishes/{Id}.128.png";
        [NotMapped]
        public string Image256 => $"/summer2021/gradeddishes/{Id}.256.png";
    }
}