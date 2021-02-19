using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public Guid SongId { get; set; }
        public Song Song { get; set; }
        
        public ICollection<Dish> Dishes { get; set; }
    }
}