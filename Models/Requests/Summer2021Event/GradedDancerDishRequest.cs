using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class GradedDancerDishRequest
    {
        public bool PairBonus { get; set; }
        public IEnumerable<ScoreSubmissionRequest> Scores { get; set; }
        public IFormFile FinalImage { get; set; }
    }
}