using System;

namespace AusDdrApi.Models.Responses
{
    public class IngredientWithGradingResponse
    {
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SongId { get; set; }
        
        public Guid GradedIngredientId { get; set; }
        public string Grade { get; set; }
        public int RequiredScore { get; set; }
        public string Description { get; set; }
    }
}