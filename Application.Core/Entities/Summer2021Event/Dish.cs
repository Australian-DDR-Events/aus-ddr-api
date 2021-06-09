using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Dish : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public int MaxScore { get; set; }
        
        public ICollection<DishSong> DishSongs { get; set; } = default!;
        public virtual ICollection<Ingredient> Ingredients { get; set; } = default!;
    }
}