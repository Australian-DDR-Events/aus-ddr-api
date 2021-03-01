using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class SongResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Level { get; set; }
        public string Image32 { get; set; } = string.Empty;
        public string Image64 { get; set; } = string.Empty;
        public string Image128 { get; set; } = string.Empty;
        public string Image256 { get; set; } = string.Empty;
        public string Image512 { get; set; } = string.Empty;
        
        public static SongResponse FromEntity(Song song) => new SongResponse
        {
            Id = song.Id,
            Name = song.Name,
            Artist = song.Artist,
            Difficulty = song.Difficulty,
            Level = song.Level,
            Image32 = $"/songs/{song.Id}.32.png",
            Image64 = $"/songs/{song.Id}.64.png",
            Image128 = $"/songs/{song.Id}.128.png",
            Image256 = $"/songs/{song.Id}.256.png",
            Image512 = $"/songs/{song.Id}.512.png",
        };

        public override bool Equals(object? comparator)
        {
            var comparatorAsSongResponse = comparator as SongResponse;
            if (comparatorAsSongResponse == null) return false;
            return Equals(comparatorAsSongResponse);
        }

        public bool Equals(SongResponse comparator)
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