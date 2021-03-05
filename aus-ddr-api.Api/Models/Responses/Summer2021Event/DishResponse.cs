using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class DishResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<DishSongResponse> Songs { get; set; } = new HashSet<DishSongResponse>();
        public IEnumerable<IngredientResponse> Ingredients { get; set; } = new HashSet<IngredientResponse>();
        public string Image32 { get; set; } = string.Empty;
        public string Image64 { get; set; } = string.Empty;
        public string Image128 { get; set; } = string.Empty;
        public string Image256 { get; set; } = string.Empty;

        public static DishResponse FromEntity(Dish dish) => new DishResponse()
        {
            Id = dish.Id,
            Name = dish.Name,
            Songs = dish.DishSongs.Select(DishSongResponse.FromEntity),
            Ingredients = dish.DishIngredients.Select(IngredientResponse.FromEntity),
            Image32 = $"/summer2021/dishes/{dish.Id}.32.png",
            Image64 = $"/summer2021/dishes/{dish.Id}.64.png",
            Image128 = $"/summer2021/dishes/{dish.Id}.128.png",
            Image256 = $"/summer2021/dishes/{dish.Id}.256.png"
        };
    }
}