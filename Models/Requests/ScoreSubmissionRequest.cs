using System;

namespace AusDdrApi.Models.Requests
{
    public class ScoreSubmissionRequest
    {
        public int Score { get; set; }
        public string ScoreUrl { get; set; }
        public Guid DancerId { get; set; }
        public Guid SongId { get; set; }
    }
}