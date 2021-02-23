using System;

namespace AusDdrApi.Models.Requests
{
    public class GradedDishRequest
    {
        public int Grade { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}