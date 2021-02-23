using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDancerDishResponse
    {
        public Guid Id { get; set; }
        public Guid GradedDishId { get; set; }
        public Guid DancerId { get; set; }

        public static GradedDancerDishResponse FromEntity(GradedDancerDish dish) => new GradedDancerDishResponse
        {
            Id = dish.Id,
            GradedDishId = dish.GradedDishId,
            DancerId = dish.DancerId
        };
    }
}