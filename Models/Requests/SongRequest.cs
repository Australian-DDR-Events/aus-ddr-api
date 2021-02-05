using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Requests
{
    public class SongRequest
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string ImageUrl { get; set; }
        public string Difficulty { get; set; }
        public int Level { get; set; }
        
        public Song ToEntity() => new Song
        {
            Name = Name,
            Artist = Artist,
            ImageUrl = ImageUrl,
            Difficulty = Difficulty,
            Level = Level
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