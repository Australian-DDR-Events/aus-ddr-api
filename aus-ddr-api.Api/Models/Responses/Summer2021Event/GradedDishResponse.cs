using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDishResponse
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;

        public static GradedDishResponse FromEntity(GradedDish gradedDish) => new GradedDishResponse()
        {
            Id = gradedDish.Id,
            Description = gradedDish.Description,
            Grade = gradedDish.Grade.ToString("g"),
        };
    }
}