using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class GradedDishResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public string Image32 { get; set; } = string.Empty;
        public string Image64 { get; set; } = string.Empty;
        public string Image128 { get; set; } = string.Empty;
        public string Image256 { get; set; } = string.Empty;

        public static GradedDishResponse FromEntity(GradedDish gradedDish) => new GradedDishResponse()
        {
            Id = gradedDish.Id,
            Name = gradedDish.Dish?.Name ?? string.Empty,
            Description = gradedDish.Description,
            Grade = gradedDish.Grade.ToString("g"),
            Image32 = $"/summer2021/gradeddishes/{gradedDish.Id}.32.png",
            Image64 = $"/summer2021/gradeddishes/{gradedDish.Id}.64.png",
            Image128 = $"/summer2021/gradeddishes/{gradedDish.Id}.128.png",
            Image256 = $"/summer2021/gradeddishes/{gradedDish.Id}.256.png"
        };
    }
}