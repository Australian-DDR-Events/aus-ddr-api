using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class EventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public static EventResponse FromEntity(Event eEvent) => new EventResponse
        {
            Id = eEvent.Id,
            Name = eEvent.Name,
            Description = eEvent.Description,
            StartDate = eEvent.StartDate,
            EndDate = eEvent.EndDate
        };
    }
}