using System;

namespace AusDdrApi.Entities
{
    public class GradedDancerIngredient
    {
        public Guid Id { get; set; }
        
        public Guid GradedIngredientId { get; set; }
        public GradedIngredient GradedIngredient { get; set; }
        
        public Guid DancerId { get; set; }
        public Dancer Dancer { get; set; }
        
        public Guid ScoreId { get; set; }
        public Song Score { get; set; }
    }
}