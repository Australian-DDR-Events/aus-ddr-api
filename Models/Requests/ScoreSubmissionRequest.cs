using System;

namespace AusDdrApi.Models.Requests
{
    public class ScoreSubmissionRequest
    {
        public int Score { get; set; }
        public string ImageUrl { get; set; }
        public Guid DancerId { get; set; }
        public Guid SongId { get; set; }
    }
}