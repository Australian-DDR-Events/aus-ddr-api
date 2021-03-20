using System;
using AusDdrApi.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace AusDdrApi.Models.Responses
{
    public class IngredientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SongResponse? Song { get; set; }
        public string Image32 { get; set; } = string.Empty;
        public string Image64 { get; set; } = string.Empty;
        public string Image128 { get; set; } = string.Empty;
        public string Image256 { get; set; } = string.Empty;
        
        public static IngredientResponse FromEntity(Ingredient ingredient) => new IngredientResponse()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Song = ingredient.Song != null ? SongResponse.FromEntity(ingredient.Song) : null,
            Image32 = $"/summer2021/ingredients/{ingredient.Id}.32.png",
            Image64 = $"/summer2021/ingredients/{ingredient.Id}.64.png",
            Image128 = $"/summer2021/ingredients/{ingredient.Id}.128.png",
            Image256 = $"/summer2021/ingredients/{ingredient.Id}.256.png"
        };
    }
}
