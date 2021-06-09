using System;
namespace Api.Core.Entities
{
    public class Score : BaseEntity
    {
        public int Value { get; set; }
        public DateTime SubmissionTime { get; set; }

        public Guid DancerId { get; set; }
        public Dancer Dancer { get; set; } = default!;
        
        public Guid SongDifficultyId { get; set; }
        public SongDifficulty SongDifficulty { get; set; } = default!;
    }
}