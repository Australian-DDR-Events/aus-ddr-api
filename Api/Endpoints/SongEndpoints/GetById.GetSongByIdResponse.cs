using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;

namespace AusDdrApi.Endpoints.SongEndpoints;

public class GetSongByIdResponse
{
    private GetSongByIdResponse(Guid id, string name, string artist, IEnumerable<SongDifficulty> songDifficulties)
    {
        Id = id;
        Name = name;
        Artist = artist;
        Difficulties = songDifficulties.Select(SongDifficultyFragment.Convert);
    }
    
    public Guid Id { get; }
    public string Name { get; }
    public string Artist { get; }
    
    public IEnumerable<SongDifficultyFragment> Difficulties { get; }

    public class SongDifficultyFragment
    {
        private SongDifficultyFragment(Guid id, PlayMode mode, Difficulty difficulty, int level)
        {
            Id = id;
            Mode = mode;
            Difficulty = difficulty;
            Level = level;
        }
        
        public Guid Id { get; }
        public PlayMode Mode { get; }
        public Difficulty Difficulty { get; }
        public int Level { get; }

        public static SongDifficultyFragment Convert(SongDifficulty songDifficulty) =>
            new(songDifficulty.Id, songDifficulty.PlayMode, songDifficulty.Difficulty,
                songDifficulty.Level);
    }

    public static GetSongByIdResponse Convert(Song song) =>
        new(song.Id, song.Name, song.Artist, song.SongDifficulties);
}
