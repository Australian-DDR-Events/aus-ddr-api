using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AusDdrApi.Entities;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Models.Requests
{
    public class SongRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Artist { get; set; } = string.Empty;
        [Required]
        public string Difficulty { get; set; } = string.Empty;
        [Required]
        public int Level { get; set; }
        [Required]
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