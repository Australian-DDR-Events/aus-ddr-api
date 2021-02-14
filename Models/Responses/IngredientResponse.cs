using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class IngredientResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public SongResponse Song { get; set; }
        
        public static IngredientResponse FromEntity(Ingredient ingredient) => new IngredientResponse()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Song = SongResponse.FromEntity(ingredient.Song),
        };
    }
}
