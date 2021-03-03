using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Requests
{
    public class SongRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Level { get; set; }
        public int MaxScore { get; set; }

        public Song ToEntity() => new Song
        {
            Name = Name,
            Artist = Artist,
            Difficulty = Difficulty,
            Level = Level,
            MaxScore = MaxScore
        };
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Level < 1 || Level > 19)
            {
                yield return new ValidationResult("Level must be between 1 and 19");
            }
        }
    }
}