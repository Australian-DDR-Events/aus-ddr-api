using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Infrastructure.Data;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Postgres database collection")]
public class SongRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly SongRepository _songRepository;

    public SongRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _songRepository = new SongRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    #region GetSongWithTopScores

    [Fact(DisplayName = "When song has no difficulties, return just song")]
    public void WhenSongHasNoDifficulties_ReturnSong()
    {
        var song = new Song
        {
            Name = "sample-name"
        };

        var addedSong = _fixture._context.Songs.Add(song);
        _fixture._context.SaveChanges();

        var result = _songRepository.GetSongWithTopScores(addedSong.Entity.Id);

        Assert.NotNull(result);
        Assert.Equal(song.Name, result.Name);
    }

    [Fact(DisplayName = "When song has difficulties with no scores, return difficulties")]
    public void WhenSongHasDifficulties_ReturnWithDifficulties()
    {
        var song = new Song
        {
            Name = "sample-name",
            SongDifficulties = new List<SongDifficulty>()
            {
                new SongDifficulty()
                {
                    Difficulty = Difficulty.BASIC
                },
                new SongDifficulty()
                {
                    Difficulty = Difficulty.CHALLENGE
                }
            }
        };

        var addedSong = _fixture._context.Songs.Add(song);
        _fixture._context.SaveChanges();

        var result = _songRepository.GetSongWithTopScores(addedSong.Entity.Id);

        Assert.NotNull(result);
        Assert.Equal(song.Name, result.Name);
        Assert.Equal(2, result.SongDifficulties.Count);
    }

    [Fact(DisplayName = "When song has difficulties with scores, top 3 scores are returned")]
    public void WhenSongHasDifficultiesWithScores_ThenProvideTop3Scores()
    {
        var dancer = new Dancer();
        dancer = _fixture._context.Dancers.Add(dancer).Entity;
        var songDifficulty1 = new SongDifficulty()
        {
            Difficulty = Difficulty.BASIC,
            Scores = Enumerable.Range(0, 10).Select(num => new Score() {Value = num, DancerId = dancer.Id}).ToList()
        };
        var songDifficulty2 = new SongDifficulty()
        {
            Difficulty = Difficulty.CHALLENGE,
            Scores = Enumerable.Range(0, 2).Select(num => new Score() {Value = num, DancerId = dancer.Id}).ToList()
        };
        var song = new Song()
        {
            Name = "sample-name",
            SongDifficulties = new List<SongDifficulty>() {songDifficulty1, songDifficulty2}
        };

        var addedSong = _fixture._context.Songs.Add(song);
        _fixture._context.SaveChanges();

        var result = _songRepository.GetSongWithTopScores(addedSong.Entity.Id);
        
        Assert.NotNull(result);
        Assert.Equal(song.Name, result.Name);
        Assert.Equal(2, result.SongDifficulties.Count);
        
        Assert.Equal(3, result.SongDifficulties.First(d => d.Difficulty.Equals(Difficulty.BASIC)).Scores.Count);
        Assert.Equal(2, result.SongDifficulties.First(d => d.Difficulty.Equals(Difficulty.CHALLENGE)).Scores.Count);
    }

    [Fact(DisplayName = "When song not found, return null")]
    public void WhenSongNotFound_ReturnNull()
    {
        var result = _songRepository.GetSongWithTopScores(Guid.NewGuid());
        Assert.Null(result);
    }

    #endregion
}