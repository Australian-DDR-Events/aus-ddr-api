using System;
using System.ComponentModel.DataAnnotations;
using AusDdrApi.Entities;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class BadgeRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        [Required]
        public IFormFile? BadgeImage { get; set; }

        public Badge ToEntity() => new Badge
        {
            Name = Name,
            Description = Description,
            EventId = EventId
        };
    }
}