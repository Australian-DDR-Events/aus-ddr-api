using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Chart : BaseEntity
    {
        public PlayMode PlayMode { get; set; }
        public Difficulty Difficulty { get; set; }
        public int Level { get; set; }
        public int MaxScore { get; set; }
        
        public Guid SongId { get; set; }
        public Song Song { get; set; } = default!;
        
        public ICollection<Score> Scores { get; set; } = default!;
        public virtual ICollection<Course> Courses { get; set; } = default!;

        public virtual ICollection<Event> Events { get; set; } = default!;
    }
    
    public enum PlayMode {
        SINGLE,
        DOUBLE
    }

    public enum Difficulty
    {
        BEGINNER,
        BASIC,
        STANDARD,
        EXPERT,
        CHALLENGE
    }
}