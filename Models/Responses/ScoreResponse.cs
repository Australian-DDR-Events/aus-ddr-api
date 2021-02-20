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
        
        public Guid SongId { get; set; }
        
        public static ScoreResponse FromEntity(Score score) => new ScoreResponse
        {
            Id = score.Id,
            Value = score.Value,
            SubmissionTime = score.SubmissionTime,
            DancerId = score.DancerId,
            SongId = score.SongId,
            ImageUrl = $"/songs/{score.SongId}/scores/{score.Id}.png"
        };
    }
}