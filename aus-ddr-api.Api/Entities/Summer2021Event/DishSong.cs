using System;

namespace AusDdrApi.Entities
{
    public class DishSong
    {
        public Guid Id { get; set; }
        public int CookingOrder { get; set; }
        public string CookingMethod { get; set; }
        
        public Guid DishId { get; set; }
        public Dish? Dish { get; set; }
        
        public Guid SongId { get; set; }
        public Song? Song { get; set; }
    }
}