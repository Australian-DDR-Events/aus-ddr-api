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
        
        #nullable enable
        public ICollection<DishSongResponse>? DishSongs { get; set; }
        public ICollection<GradedDishResponse>? GradedDishes { get; set; }
        #nullable disable

        public static DishResponse FromEntity(Dish dish, ICollection<GradedDish>? gradedDishes = null) => new DishResponse()
        {
            Id = dish.Id,
            Name = dish.Name,
            DishSongs = dish.DishSongs?.Select(DishSongResponse.FromEntity).ToArray(),
            GradedDishes = gradedDishes?.Select(GradedDishResponse.FromEntity).ToArray()
        };
    }
}