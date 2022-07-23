using System;
using System.Collections.Generic;
using System.Threading;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Services;
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
}