using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Song repository collection")]
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

        var result = _songRepository.GetSong(addedSong.Entity.Id);

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

        var result = _songRepository.GetSong(addedSong.Entity.Id);

        Assert.NotNull(result);
        Assert.Equal(song.Name, result.Name);
        Assert.Equal(2, result.SongDifficulties.Count);

        Assert.Empty(result.SongDifficulties.First(d => d.Difficulty.Equals(Difficulty.BASIC)).Scores);
        Assert.Empty(result.SongDifficulties.First(d => d.Difficulty.Equals(Difficulty.CHALLENGE)).Scores);
    }

    [Fact(DisplayName = "When song not found, return null")]
    public void WhenSongNotFound_ReturnNull()
    {
        var result = _songRepository.GetSong(Guid.NewGuid());
        Assert.Null(result);
    }

    #endregion

    #region GetSongs

    [Fact(DisplayName = "When limit, only fetch limit")]
    public void GetSongs_SongsFound_LimitToN()
    {
        var songs = new List<Song> {SongGenerator.CreateSong(), SongGenerator.CreateSong(), SongGenerator.CreateSong()};
        _fixture._context.Songs.AddRange(songs);
        _fixture._context.SaveChanges();

        var result = _songRepository.GetSongs(0, 2);
        
        Assert.Equal(2, result.Count());
    }

    [Fact(DisplayName = "When skip, only fetch up to end of songs")]
    public void GetSongs_SongsFound_SkipN()
    {
        var songs = new List<Song> {SongGenerator.CreateSong(), SongGenerator.CreateSong(), SongGenerator.CreateSong(), SongGenerator.CreateSong()};
        _fixture._context.Songs.AddRange(songs);
        _fixture._context.SaveChanges();

        var result = _songRepository.GetSongs(2, 4);
        
        Assert.Equal(2, result.Count());
    }

    #endregion

    #region CreateSong

    [Fact(DisplayName = "When create, can fetch song from table")]
    public async Task CreateSong_SongIsCreated()
    {
        var song = SongGenerator.CreateSong();

        await _songRepository.CreateSong(song, CancellationToken.None);

        var songResult = _fixture._context.Songs.First(s => s.Id.Equals(song.Id));
        
        Assert.NotNull(songResult);
    }

    #endregion
}