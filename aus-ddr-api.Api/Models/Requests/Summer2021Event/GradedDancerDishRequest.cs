using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class GradedDancerDishRequest
    {
        [Required]
        public bool PairBonus { get; set; }
        [Required]
        public IEnumerable<ScoreSubmissionRequest> Scores { get; set; } = new List<ScoreSubmissionRequest>();
        [Required]
        public IFormFile? FinalImage { get; set; }
    }
}