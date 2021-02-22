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

        public static DishResponse FromEntity(Dish dish) => new DishResponse()
        {
            Id = dish.Id,
            Name = dish.Name,
        };
    }
}