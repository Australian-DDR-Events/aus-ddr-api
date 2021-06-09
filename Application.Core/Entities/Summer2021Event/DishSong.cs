using System;

namespace Application.Core.Entities
{
    public class DishSong : BaseEntity
    {
        public int CookingOrder { get; set; }
        public string CookingMethod { get; set; } = string.Empty;
        
        public Guid DishId { get; set; }
        public Dish? Dish { get; set; }
        
        public Guid SongDifficultyId { get; set; }
        public SongDifficulty? SongDifficulty { get; set; }
    }
}