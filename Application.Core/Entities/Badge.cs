using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Badge : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Threshold { get; set; } = null;
        
        public Guid EventId { get; set; }
        public Event? Event { get; set; }
        
        public virtual ICollection<Dancer> Dancers { get; set; } = default!;
    }
}