using System;

namespace Application.Core.Entities
{
    public class GradedDancerIngredient : BaseEntity
    {
        public Guid GradedIngredientId { get; set; }
        public GradedIngredient GradedIngredient { get; set; } = default!;
        
        public Guid DancerId { get; set; }
        public Dancer Dancer { get; set; } = default!;
        
        public Guid ScoreId { get; set; }
        public Score Score { get; set; } = default!;
    }
}