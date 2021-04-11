using System;
using System.ComponentModel.DataAnnotations;
using AusDdrApi.Entities;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class IngredientRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public Guid SongId { get; set; }
        [Required]
        public IFormFile? IngredientImage { get; set; }

        public Ingredient ToEntity() => new Ingredient()
        {
            Name = Name,
            SongId = SongId
        };
    }
}