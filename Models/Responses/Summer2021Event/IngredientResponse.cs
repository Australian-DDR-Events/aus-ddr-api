using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class IngredientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        #nullable enable
        public SongResponse? Song { get; set; }
        #nullable disable
        
        public static IngredientResponse FromEntity(Ingredient ingredient) => new IngredientResponse()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Song = ingredient.Song != null ? SongResponse.FromEntity(ingredient.Song) : null,
        };
    }
}
