using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDancerIngredientResponse
    {
        public Guid Id { get; set; }
        
        public GradedIngredientResponse GradedIngredient { get; set; }
        
        public DancerResponse Dancer { get; set; }
        
        public ScoreResponse Score { get; set; }

        public static GradedDancerIngredientResponse FromEntity(GradedDancerIngredient entity) => new GradedDancerIngredientResponse()
        {
            Id = entity.Id,
            GradedIngredient = GradedIngredientResponse.FromEntity(entity.GradedIngredient),
            Dancer = DancerResponse.FromEntity(entity.Dancer),
            Score = ScoreResponse.FromEntity(entity.Score),
        };
    }
}
