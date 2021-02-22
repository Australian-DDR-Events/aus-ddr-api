using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class ScoreSubmissionRequest
    {
        [Required]
        public int Score { get; set; }
        [Required]
        public IFormFile? ScoreImage { get; set; }
        [Required]
        public Guid SongId { get; set; }
    }
}