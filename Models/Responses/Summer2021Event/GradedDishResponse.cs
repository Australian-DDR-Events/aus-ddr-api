using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDishResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
        #nullable enable
        public DishResponse? Dish { get; set; }
        #nullable disable

        public static GradedDishResponse FromEntity(GradedDish gradedDish) => new GradedDishResponse()
        {
            Id = gradedDish.Id,
            Description = gradedDish.Description,
            Grade = gradedDish.Grade.ToString("g"),
            Dish = gradedDish.Dish != null ? DishResponse.FromEntity(gradedDish.Dish) : null
        };
    }
}