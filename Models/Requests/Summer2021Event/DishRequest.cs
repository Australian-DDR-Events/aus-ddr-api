using System;
using System.Collections.Generic;

namespace AusDdrApi.Models.Requests
{
    public class DishRequest
    {
        public string Name { get; set; }
        
        public ICollection<Guid> SongIds { get; set; }
        public ICollection<Guid> IngredientIds { get; set; }
    }
}