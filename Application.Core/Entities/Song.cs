using System;
using System.Collections.Generic;

namespace Application.Core.Entities
{
    public class Song : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        
        public ICollection<SongDifficulty> SongDifficulties { get; set; } = default!;
    }
}