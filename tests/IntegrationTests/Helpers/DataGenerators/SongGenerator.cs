using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class SongGenerator
{
    public static Song CreateSong() => new Song
    {
        Id = Guid.NewGuid()
    };
}