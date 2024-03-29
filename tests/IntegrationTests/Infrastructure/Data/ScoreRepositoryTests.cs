using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Score repository collection")]
public class ScoreRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly ScoreRepository _scoreRepository;

    public ScoreRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _scoreRepository = new ScoreRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
        _fixture._context.ChangeTracker.Clear();
    }

    private void AddSongDiffToTable(Chart s)
    {
        _fixture._context.Charts.Add(s);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();
    }

    #region CreateScore

    [Fact(DisplayName = "When all required entities exist, score is added")]
    public async Task CreateScore_AllRequiredEntitiesExist_ScoreCreated()
    {
        var song = SongGenerator.CreateSong();
        _fixture._context.Songs.Add(song);

        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Dancers.Add(dancer);
        
        var songDiff = ChartGenerator.CreateChart(song);
        AddSongDiffToTable(songDiff);
        
        var score = new Score() {Id = Guid.NewGuid()};

        var response = await _scoreRepository.CreateScore(songDiff.Id, dancer.Id, score, null);
        var data = _fixture._context.Scores.First(s => s.Id.Equals(score.Id));
        
        Assert.True(response);
        Assert.Equal(score.Id, data.Id);
        Assert.Null(data.EventId);
    }

    [Fact(DisplayName = "When all required entities exist, with event, score is added with event")]
    public async Task CreateScore_AllRequiredEntitiesExistWithEvent_ScoreCreated()
    {
        var song = SongGenerator.CreateSong();
        _fixture._context.Songs.Add(song);

        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Dancers.Add(dancer);

        var e = EventGenerator.CreateEvent();
        _fixture._context.Events.Add(e);
        
        var songDiff = ChartGenerator.CreateChart(song);
        AddSongDiffToTable(songDiff);
        
        var score = new Score() {Id = Guid.NewGuid()};

        var response = await _scoreRepository.CreateScore(songDiff.Id, dancer.Id, score, e.Id);
        var data = _fixture._context.Scores.First(s => s.Id.Equals(score.Id));
        
        Assert.True(response);
        Assert.Equal(score.Id, data.Id);
        Assert.Equal(e.Id, data.EventId);
    }

    [Fact(DisplayName = "When score fails to add, returns false")]
    public async Task CreateScore_ScoreFailsToCreate_False()
    {
        var song = SongGenerator.CreateSong();
        _fixture._context.Songs.Add(song);

        var dancer = DancerGenerator.CreateDancer();
        _fixture._context.Dancers.Add(dancer);

        var songDiff = ChartGenerator.CreateChart(song);
        AddSongDiffToTable(songDiff);
        
        var score = new Score() {Id = Guid.NewGuid()};

        var firstResponse = await _scoreRepository.CreateScore(songDiff.Id, dancer.Id, score, null);
        var secondResponse = await _scoreRepository.CreateScore(songDiff.Id, dancer.Id, score, null);
        
        Assert.True(firstResponse);
        Assert.False(secondResponse);
    }

    #endregion
}