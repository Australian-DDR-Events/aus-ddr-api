using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Requests
{
    public class EventRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Event ToEntity() => new Event
        {
            Name = Name,
            Description = Description,
            StartDate = StartDate,
            EndDate = EndDate
        };
    }
}