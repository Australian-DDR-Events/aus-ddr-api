using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Badge
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public Guid EventId { get; set; }
        public Event? Event { get; set; }

        public virtual ICollection<Dancer> Dancers { get; set; } = new HashSet<Dancer>();
    }
}