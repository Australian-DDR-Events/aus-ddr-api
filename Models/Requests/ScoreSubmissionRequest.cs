using System;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class ScoreSubmissionRequest
    {
        public int Score { get; set; }
        
        public IFormFile ScoreImage { get; set; }
        public Guid DancerId { get; set; }
        public Guid SongId { get; set; }
    }
}