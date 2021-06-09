using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class GradedDancerDish : BaseEntity
    {
        public Guid GradedDishId { get; set; }
        public GradedDish? GradedDish { get; set; }
        
        public Guid DancerId { get; set; }
        public Dancer? Dancer { get; set; }
        
        public ICollection<Score> Scores { get; set; } = default!;
    }
}