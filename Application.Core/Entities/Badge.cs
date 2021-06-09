using System;

namespace Api.Core.Entities
{
    public class Badge : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public Guid EventId { get; set; }
        public Event? Event { get; set; }
    }
}