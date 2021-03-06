using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDancerIngredientResponse
    {
        public Guid Id { get; set; }
        public Guid DancerId { get; set; }
        public GradedIngredientResponse? GradedIngredient { get; set; }
        public ScoreResponse? Score { get; set; }
        
        public static GradedDancerIngredientResponse FromEntity(GradedDancerIngredient entity) => new GradedDancerIngredientResponse()
        {
            Id = entity.Id,
            DancerId = entity.DancerId,
            GradedIngredient = entity.GradedIngredient != null ? GradedIngredientResponse.FromEntity(entity.GradedIngredient) : null,
            Score = entity.Score != null ? ScoreResponse.FromEntity(entity.Score) : null
        };
    }
}
