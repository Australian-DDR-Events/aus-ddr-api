using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class GradedDancerIngredientSubmissionRequest
    {
        public int Score { get; set; }
        
        public IFormFile ScoreImage { get; set; }
        public Guid IngredientId { get; set; }
    }
}