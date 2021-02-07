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
        
        public string ImageUrl { get; set; }
        
        public Guid DancerId { get; set; }

        public SongResponse Song { get; set; }
        
        public static ScoreResponse FromEntity(Score score) => new ScoreResponse
        {
            Id = score.Id,
            Value = score.Value,
            SubmissionTime = score.SubmissionTime,
            DancerId = score.DancerId,
            Song = SongResponse.FromEntity(score.Song),
            ImageUrl = $"/Songs/{score.SongId}/Scores/{score.Id}.png"
        };
    }
}