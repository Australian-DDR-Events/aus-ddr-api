using System;
using System.Collections.Generic;

namespace AusDdrApi.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public virtual ICollection<SongDifficulty> SongDifficulties { get; set; } = new HashSet<SongDifficulty>();
    }
}