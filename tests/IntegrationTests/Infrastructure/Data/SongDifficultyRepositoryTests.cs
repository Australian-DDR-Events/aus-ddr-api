using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Song difficulty repository collection")]
public class SongDifficultyRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly SongDifficultyRepository _songDifficultyRepository;

    public SongDifficultyRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _songDifficultyRepository = new SongDifficultyRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
    }

    #region CreateSongDifficulty

    [Fact(DisplayName = "When song exists, difficulty is created")]
    public async Task WhenSongCreated_DifficultyCreated()
    {
        var song = SongGenerator.CreateSong();
        _fixture._context.Songs.Add(song);
        await _fixture._context.SaveChangesAsync();
        
        var songDifficulty = SongDifficultyGenerator.CreateSongDifficulty(song);
        
        var result = await _songDifficultyRepository.CreateSongDifficulty(song.Id, songDifficulty, CancellationToken.None);

        var databaseResult = _fixture._context.SongDifficulties.FirstOrDefault(s => s.Id.Equals(songDifficulty.Id));
        
        Assert.True(result);

        Assert.NotNull(databaseResult);
        Assert.Equal(songDifficulty.Id, databaseResult.Id);
        Assert.Equal(song.Id, databaseResult.SongId);
    }

    [Fact(DisplayName = "When song does not exist, return false")]
    public async Task WhenSongNotExist_False()
    {
        var song = SongGenerator.CreateSong();
        var songDifficulty = SongDifficultyGenerator.CreateSongDifficulty(song);
        
        var result = await _songDifficultyRepository.CreateSongDifficulty(Guid.NewGuid(), songDifficulty, CancellationToken.None);

        var databaseResult = _fixture._context.SongDifficulties.FirstOrDefault(s => s.Id.Equals(songDifficulty.Id));
        
        Assert.False(result);
        Assert.Null(databaseResult);
    }

    #endregion
}