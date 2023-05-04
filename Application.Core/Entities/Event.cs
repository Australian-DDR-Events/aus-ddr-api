using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Interfaces;

namespace Application.Core.Entities
{
    public class Event : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Chart> Charts { get; set; } = default!;
        public virtual ICollection<Course> Courses { get; set; } = default!;
    }
}
