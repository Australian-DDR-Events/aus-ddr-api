using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class DishResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<DishSongResponse> DishSongs { get; set; }
        public ICollection<IngredientResponse> Ingredients { get; set; }

        public static DishResponse FromEntity(Dish dish) => new DishResponse()
        {
            Id = dish.Id,
            Name = dish.Name,
            DishSongs = dish.DishSongs.Select(DishSongResponse.FromEntity).ToArray(),
            Ingredients = dish.Ingredients.Select(IngredientResponse.FromEntity).ToArray()
        };
    }
}