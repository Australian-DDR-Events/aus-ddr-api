using System;

namespace AusDdrApi.Entities
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Level { get; set; }
        
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
                Level == comparator.Level);
        }

        public override int GetHashCode()
        {
            return (Id, Name, Artist, Difficulty, Level).GetHashCode();
        }
    }
}