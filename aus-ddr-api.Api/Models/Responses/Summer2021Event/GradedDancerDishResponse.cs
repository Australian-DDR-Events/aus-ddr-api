using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDancerDishResponse
    {
        public Guid Id { get; set; }
        public Guid GradedDishId { get; set; }
        public Guid DancerId { get; set; }

        public string ResultImage { get; set; } = string.Empty;

        public static GradedDancerDishResponse FromEntity(GradedDancerDish dish, Guid dishId) => new GradedDancerDishResponse
        {
            Id = dish.Id,
            GradedDishId = dish.GradedDishId,
            DancerId = dish.DancerId,
            ResultImage = $"dishes/{dishId}/final/{dish.Id}.png"
        };
    }
}