using System;
using System.Collections.Generic;
using Application.Core.Interfaces;

namespace Application.Core.Entities
{
    public class Badge : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Threshold { get; set; } = null;
        
        public Guid? EventId { get; set; }
        public Event? Event { get; set; }
        
        public virtual ICollection<Dancer> Dancers { get; set; } = default!;
    }
}