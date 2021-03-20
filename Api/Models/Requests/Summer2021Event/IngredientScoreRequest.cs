using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class IngredientScoreRequest
    {
        [Required]
        public int Score { get; set; }
        [Required]
        public IFormFile? ScoreImage { get; set; }
    }
}