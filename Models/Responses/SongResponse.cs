using System;
using AusDdrApi.Entities;

namespace AusDdrApi.Models.Responses
{
    public class SongResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Level { get; set; }
        
        public static SongResponse FromEntity(Song song) => new SongResponse
        {
            Id = song.Id,
            Name = song.Name,
            Artist = song.Artist,
            ImageUrl = song.ImageUrl,
            Difficulty = song.Difficulty,
            Level = song.Level
        };
    }
}