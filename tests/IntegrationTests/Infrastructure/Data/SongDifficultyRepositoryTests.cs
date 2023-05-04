using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Chart repository collection")]
public class ChartRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly ChartRepository _chartRepository;

    public ChartRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _chartRepository = new ChartRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    #region CreateChart

    [Fact(DisplayName = "When song exists, chart is created")]
    public async Task WhenSongCreated_ChartCreated()
    {
        var song = SongGenerator.CreateSong();
        _fixture._context.Songs.Add(song);
        await _fixture._context.SaveChangesAsync();
        
        var chart = ChartGenerator.CreateChart(song);
        
        var result = await _chartRepository.CreateChart(song.Id, chart, CancellationToken.None);

        var databaseResult = _fixture._context.Charts.FirstOrDefault(s => s.Id.Equals(chart.Id));
        
        Assert.True(result);

        Assert.NotNull(databaseResult);
        Assert.Equal(chart.Id, databaseResult.Id);
        Assert.Equal(song.Id, databaseResult.SongId);
    }

    [Fact(DisplayName = "When song does not exist, return false")]
    public async Task WhenSongNotExist_False()
    {
        var song = SongGenerator.CreateSong();
        var chart = ChartGenerator.CreateChart(song);
        
        var result = await _chartRepository.CreateChart(Guid.NewGuid(), chart, CancellationToken.None);

        var databaseResult = _fixture._context.Charts.FirstOrDefault(s => s.Id.Equals(chart.Id));
        
        Assert.False(result);
        Assert.Null(databaseResult);
    }

    #endregion
}