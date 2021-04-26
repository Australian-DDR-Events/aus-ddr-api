using System;
using System.ComponentModel.DataAnnotations.Schema;
using AusDdrApi.GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;

namespace AusDdrApi.Entities
{
    public class Score
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// The score that 
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Stores the date time the score is submitted.<br/>
        /// This value is generated once on create and cannot be updated.
        /// </summary>
        public DateTime SubmissionTime { get; set; }

        public Guid DancerId { get; set; }
        
        [GraphQLType(typeof(NonNullType<DancerType>))]
        public Dancer Dancer { get; set; } = default!;
        
        public Guid SongDifficultyId { get; set; }
        
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<SongDifficultyType>>>))]
        public SongDifficulty SongDifficulty { get; set; } = default!;

        [NotMapped] public string ImageUrl => $"/songs/{SongDifficultyId}/scores/{Id}.png";
    }
}