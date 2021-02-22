using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class GradedDancerDish
    {
        public Guid Id { get; set; }
        
        public Guid GradedDishId { get; set; }
        public GradedDish? GradedDish { get; set; }
        
        public Guid DancerId { get; set; }
        public Dancer? Dancer { get; set; }

        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}