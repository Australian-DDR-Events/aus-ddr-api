using System;
using System.Collections.Generic;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Services;
using Ardalis.Result;
using Moq;
using Xunit;

namespace UnitTests.Core.Services;

public class SongServiceTests
{
    private readonly Mock<IAsyncRepository<Song>> _songRepository2;
    private readonly Mock<ISongRepository> _songRepository;
    private readonly SongService _songService;

    public SongServiceTests()
    {
        _songRepository = new Mock<ISongRepository>();
        _songRepository2 = new Mock<IAsyncRepository<Song>>();
        _songService = new SongService(_songRepository2.Object, _songRepository.Object);
    }

    #region GetSongs Tests

    [Fact(DisplayName = "Returns repository result")]
    public void GetSongs_RepositoryReturnsSongs_ReturnSongs()
    {
        var songs = new List<Song>
        {
            new Song
            {
                Id = Guid.NewGuid()
            }
        };

        _songRepository.Setup(r =>
            r.GetSongs(It.IsAny<int>(), It.IsAny<int>())
        ).Returns(songs);

        var result = _songService.GetSongs(3, 5);
        
        Assert.Single(result);
        _songRepository.Verify(r =>
            r.GetSongs(
                It.Is<int>(value => value == 15),
                It.Is<int>(value => value == 5)),
            Times.Once);
    }
    
    #endregion

    #region GetSong

    [Fact(DisplayName = "When song exists, return success with song")]
    public void GetSong_RepositoryReturnsSong_ReturnSuccessResult()
    {
        var song = new Song
        {
            Id = Guid.NewGuid()
        };
        _songRepository.Setup(r =>
            r.GetSong(It.IsAny<Guid>())
        ).Returns(song);

        var result = _songService.GetSong(song.Id);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(song.Id, result.Value.Id);
        
        _songRepository.Verify(r =>
                r.GetSong(It.Is<Guid>(value => value.Equals(song.Id))),
            Times.Once);
    }

    [Fact(DisplayName = "When song not found, return not found")]
    public void GetSong_SongNotFound_ReturnNotFound()
    {
        _songRepository.Setup(r =>
            r.GetSong(It.IsAny<Guid>())
        ).Returns(null as Song);

        var result = _songService.GetSong(Guid.NewGuid());
        
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    #endregion
}