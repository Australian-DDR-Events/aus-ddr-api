using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class SongDifficultyGenerator
{
    public static SongDifficulty CreateSongDifficulty(Song song) => new SongDifficulty
    {
        Id = Guid.NewGuid(),
        Song = song,
        SongId = song.Id
    };
}