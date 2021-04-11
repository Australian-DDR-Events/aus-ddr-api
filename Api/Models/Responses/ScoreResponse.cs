using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class ScoreResponse
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// The score that 
        /// </summary>
        public int Value { get; set; }

        public DateTime SubmissionTime { get; set; }
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public Guid? DancerId { get; set; }
        public DancerResponse? Dancer { get; set; }
        public Guid? SongId { get; set; }
        public SongResponse? Song { get; set; }
        
        public static ScoreResponse FromEntity(Score score) => new ScoreResponse
        {
            Id = score.Id,
            Value = score.Value,
            SubmissionTime = score.SubmissionTime,
            Dancer = score.Dancer != null ? DancerResponse.FromEntity(score.Dancer) : null,
            DancerId = score.Dancer == null ? score.DancerId : default(Guid?),
            Song = score.Song != null ? SongResponse.FromEntity(score.Song) : null,
            SongId = score.Song == null ? score.SongId : default(Guid?),
            ImageUrl = $"/songs/{score.SongId}/scores/{score.Id}.png"
        };
    }
}