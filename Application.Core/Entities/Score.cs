using System;
namespace Application.Core.Entities
{
    public class Score : BaseEntity
    {
        public int Value { get; set; }
        public int ExScore { get; set; }
        public DateTime SubmissionTime { get; set; }

        public Guid DancerId { get; set; }
        public Dancer Dancer { get; set; } = default!;
        
        public Guid ChartId { get; set; }
        public Chart Chart { get; set; } = default!;
        
        public Guid? EventId { get; set; } = null;
        public Event? Event { get; set; } = null;
    }
}