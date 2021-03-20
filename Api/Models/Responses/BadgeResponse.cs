using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class BadgeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EventResponse? Event { get; set; }
        public string Image32 { get; set; } = string.Empty;
        public string Image64 { get; set; } = string.Empty;
        public string Image128 { get; set; } = string.Empty;
        public string Image256 { get; set; } = string.Empty;

        public static BadgeResponse FromEntity(Badge badge) => new BadgeResponse
        {
            Id = badge.Id,
            Name = badge.Name,
            Description = badge.Description,
            Event = badge.Event != null ? EventResponse.FromEntity(badge.Event) : null,
            Image32 = $"/badges/{badge.Id}.32.png",
            Image64 = $"/badges/{badge.Id}.64.png",
            Image128 = $"/badges/{badge.Id}.128.png",
            Image256 = $"/badges/{badge.Id}.256.png",
        };
    }
}