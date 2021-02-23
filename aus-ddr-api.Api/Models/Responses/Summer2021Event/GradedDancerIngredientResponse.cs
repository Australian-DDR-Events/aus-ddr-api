using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDancerIngredientResponse
    {
        public Guid Id { get; set; }
        public Guid GradedIngredientId { get; set; }
        public Guid DancerId { get; set; }
        public Guid ScoreId { get; set; }

        public static GradedDancerIngredientResponse FromEntity(GradedDancerIngredient entity) => new GradedDancerIngredientResponse()
        {
            Id = entity.Id,
            GradedIngredientId = entity.GradedIngredientId,
            DancerId = entity.DancerId,
            ScoreId = entity.ScoreId
        };
    }
}
