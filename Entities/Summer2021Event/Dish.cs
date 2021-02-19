using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<DishSong> DishSongs { get; set; }
    }
}