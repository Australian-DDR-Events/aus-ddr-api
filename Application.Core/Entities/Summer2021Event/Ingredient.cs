using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        
        public Guid SongDifficultyId { get; set; }
        public SongDifficulty? SongDifficulty { get; set; }
        
        public virtual ICollection<Dish> Dishes { get; set; } = default!;
    }
}