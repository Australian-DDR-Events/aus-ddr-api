using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class ChartGenerator
{
    public static Chart CreateChart(Song song) => new Chart
    {
        Id = Guid.NewGuid(),
        Song = song,
        SongId = song.Id
    };
}