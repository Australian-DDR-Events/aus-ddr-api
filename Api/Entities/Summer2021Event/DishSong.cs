using System;

namespace AusDdrApi.Entities
{
    public class DishSong
    {
        public Guid Id { get; set; }
        public int CookingOrder { get; set; }
        public string CookingMethod { get; set; } = string.Empty;
        
        public Guid DishId { get; set; }
        public Dish? Dish { get; set; }
        
        public Guid SongDifficultyId { get; set; }
        public SongDifficulty? SongDifficulty { get; set; }
    }
}