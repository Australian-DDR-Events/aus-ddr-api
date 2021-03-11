using System;
using System.Collections.Generic;
using System.Linq;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDancerDishResponse
    {
        public Guid Id { get; set; }
        public GradedDishResponse? GradedDish { get; set; }
        public Guid DancerId { get; set; }
        public string ResultImage { get; set; } = string.Empty;
        public IEnumerable<ScoreResponse> Scores { get; set; } = new List<ScoreResponse>();

        public static GradedDancerDishResponse FromEntity(GradedDancerDish dish) => new GradedDancerDishResponse
        {
            Id = dish.Id,
            GradedDish = dish.GradedDish != null ? GradedDishResponse.FromEntity(dish.GradedDish) : null,
            DancerId = dish.DancerId,
            ResultImage = $"dishes/{dish.GradedDish!.DishId}/final/{dish.Id}.png",
            Scores = dish.Scores.Select(ScoreResponse.FromEntity)
        };
    }
}