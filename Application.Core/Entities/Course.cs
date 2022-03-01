using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public virtual ICollection<SongDifficulty> SongDifficulties { get; set; } = default!;

        public virtual ICollection<Event> Events { get; set; } = default!;
    }
}