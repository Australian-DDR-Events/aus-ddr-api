using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate.Data;

namespace AusDdrApi.Entities
{
    public class Song
    {
        [IsProjected(true)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Level { get; set; }
        public int MaxScore { get; set; }
        
        [NotMapped]
        public string Image32 => $"/songs/{Id}.32.png";
        [NotMapped]
        public string Image64 => $"/songs/{Id}.64.png";
        [NotMapped]
        public string Image128 => $"/songs/{Id}.128.png";
        [NotMapped]
        public string Image256 => $"/songs/{Id}.256.png";
        [NotMapped]
        public string Image512 => $"/songs/{Id}.512.png";
        
        [UseFiltering]
        [UseSorting]
        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        
        [UseFiltering]
        [UseSorting]
        public ICollection<Score> Scores { get; set; } = new List<Score>();

        public override bool Equals(object? comparator)
        {
            var comparatorAsSong = comparator as Song;
            if (comparatorAsSong == null) return false;
            return Equals(comparatorAsSong);
        }

        public bool Equals(Song comparator)
        {
            return (
                Id == comparator.Id &&
                Name == comparator.Name &&
                Artist == comparator.Artist &&
                Difficulty == comparator.Difficulty &&
                Level == comparator.Level &&
                MaxScore == comparator.MaxScore);
        }

        public override int GetHashCode()
        {
            return (Id, Name, Artist, Difficulty, Level, MaxScore).GetHashCode();
        }
    }
}