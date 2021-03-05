using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int MaxScore { get; set; }
        
        public ICollection<DishSong> DishSongs { get; set; } = new List<DishSong>();
        public virtual ICollection<Ingredient> DishIngredients { get; set; } = new HashSet<Ingredient>();
    }
}