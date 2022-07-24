using System;
using System.Collections.Generic;
using Application.Core.Interfaces;

namespace Application.Core.Entities
{
    public class Song : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;

        public string KonamiId { get; set; } = string.Empty;
        
        public ICollection<SongDifficulty> SongDifficulties { get; set; } = default!;
    }
}