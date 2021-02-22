using System;
using System.Collections.Generic;

namespace AusDdrApi.Models.Requests
{
    public class DishRequest
    {
        public string Name { get; set; } = string.Empty;
        
        public ICollection<Guid> SongIds { get; set; } = new List<Guid>();
        public ICollection<Guid> IngredientIds { get; set; } = new List<Guid>();
    }
}