using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class GradedDancerIngredientSubmissionRequest
    {
        [Required]
        public int Score { get; set; }
        [Required]
        public IFormFile? ScoreImage { get; set; }
        [Required]
        public Guid IngredientId { get; set; }
    }
}