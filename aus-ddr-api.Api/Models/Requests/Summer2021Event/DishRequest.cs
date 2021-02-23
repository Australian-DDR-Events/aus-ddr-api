using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class DishRequest
    {
        public string Name { get; set; } = string.Empty;
        
        public ICollection<Guid> SongIds { get; set; } = new List<Guid>();
        public ICollection<Guid> IngredientIds { get; set; } = new List<Guid>();
        [Required]
        public IFormFile? DishImage { get; set; }
    }
}