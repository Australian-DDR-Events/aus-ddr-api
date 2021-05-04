using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AusDdrApi.GraphQL.Types;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

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
        
        [IsProjected(true)]
        public Guid SongId { get; set; }
        public Song Song { get; set; } = default!;

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ScoreType>>>))]
        [UseFiltering] [UseSorting] public ICollection<Score> Scores { get; set; } = new List<Score>();

        /// <summary>
        /// Contains the top score for each dancer that has
        /// submitted a score for this song. Resolved by
        /// graphql, not mapped by entity framework.
        /// </summary>
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<ScoreType>>>))]
        [NotMapped]
        [UseFiltering]
        [UseSorting]
        public IEnumerable<Score> DancerTopScores { get; set; } = default!;

        [NotMapped] public Score? TopScore { get; set; } = null;

        [GraphQLType(typeof(NonNullType<ListType<NonNullType<CourseType>>>))]
        [UseFiltering]
        [UseSorting]
        public virtual ICollection<Course> Courses { get; set; } = default!;
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