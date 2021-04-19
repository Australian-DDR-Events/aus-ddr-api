using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate.Data;

namespace AusDdrApi.Entities
{
    public class SongDifficulty
    {
        [IsProjected(true)]
        public Guid Id { get; set; }
        public PlayMode PlayMode { get; set; }
        public Difficulty Difficulty { get; set; }
        public int Level { get; set; }
        public int MaxScore { get; set; }
        
        public Guid SongId { get; set; }
        public Song Song { get; set; } = new Song();
        
        [UseFiltering]
        [UseSorting]
        public ICollection<Score> Scores { get; set; } = new List<Score>();

        /// <summary>
        /// Contains the top score for each dancer that has
        /// submitted a score for this song. Resolved by
        /// graphql, not mapped by entity framework.
        /// </summary>
        [NotMapped]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Score> DancerTopScores { get; set; } = new List<Score>();

        [NotMapped] public Score? TopScore { get; set; } = null;
        
        [UseFiltering]
        [UseSorting]
        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
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