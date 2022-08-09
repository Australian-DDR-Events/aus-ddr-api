using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Application.Core.Models.SongDifficulties;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace UnitTests.Api.Endpoints;
using SongDifficultyEndpoints = AusDdrApi.Endpoints.SongDifficultyEndpoints;

public class SongDifficultyTests
{
    private readonly Mock<ISongDifficultyService> _songDifficultyService = new();

    #region Create

    [Fact(DisplayName = "Create When valid request, return Accepted")]
    public async Task Create_WhenValidRequest_Accepted()
    {
        var request = new SongDifficultyEndpoints.CreateSongDifficultyRequest
        {
            SongId = Guid.NewGuid(),
            Difficulty = "EXPERT",
            Mode = "SINGLE",
            Level = 1,
            MaxScore = 1
        };
        var endpoint = new SongDifficultyEndpoints.Create(_songDifficultyService.Object);

        _songDifficultyService.Setup(s =>
            s.CreateSongDifficulty(
                It.IsAny<CreateSongDifficultyRequestModel>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        var result = await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.IsType<AcceptedResult>(result);
        
        _songDifficultyService.Verify(s =>
                s.CreateSongDifficulty(
                    It.Is<CreateSongDifficultyRequestModel>(model =>
                        model.SongId.Equals(request.SongId) &&
                        model.Difficulty == Difficulty.EXPERT &&
                        model.Mode == PlayMode.SINGLE &&
                        model.Level == 1 &&
                        model.MaxScore == 1
                    ),
                    It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = "Create When request fails in service, return BadRequest")]
    public async Task Create_WhenRequestFailsInService_BadRequest()
    {
        var request = new SongDifficultyEndpoints.CreateSongDifficultyRequest
        {
            SongId = Guid.NewGuid(),
            Difficulty = "EXPERT",
            Mode = "SINGLE",
            Level = 1,
            MaxScore = 1
        };
        var endpoint = new SongDifficultyEndpoints.Create(_songDifficultyService.Object);

        _songDifficultyService.Setup(s =>
            s.CreateSongDifficulty(
                It.IsAny<CreateSongDifficultyRequestModel>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var result = await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.IsType<BadRequestResult>(result);
        
        _songDifficultyService.Verify(s =>
                s.CreateSongDifficulty(
                    It.Is<CreateSongDifficultyRequestModel>(model =>
                        model.SongId.Equals(request.SongId) &&
                        model.Difficulty == Difficulty.EXPERT &&
                        model.Mode == PlayMode.SINGLE &&
                        model.Level == 1 &&
                        model.MaxScore == 1
                    ),
                    It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact(DisplayName = "Create When invalid Mode is provided, return BadRequest")]
    public async Task Create_InvalidModeProvided_BadRequest()
    {
        var request = new SongDifficultyEndpoints.CreateSongDifficultyRequest
        {
            SongId = Guid.NewGuid(),
            Difficulty = "EXPERT",
            Mode = "INVALID",
            Level = 1,
            MaxScore = 1
        };
        var endpoint = new SongDifficultyEndpoints.Create(_songDifficultyService.Object);

        var result = await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.IsType<BadRequestResult>(result);
        
        _songDifficultyService.Verify(s =>
            s.CreateSongDifficulty(It.IsAny<CreateSongDifficultyRequestModel>(), It.IsAny<CancellationToken>()
            ), Times.Never
        );
    }

    [Fact(DisplayName = "Create When invalid Difficulty is provided, return BadRequest")]
    public async Task Create_InvalidDifficultyProvided_BadRequest()
    {
        var request = new SongDifficultyEndpoints.CreateSongDifficultyRequest
        {
            SongId = Guid.NewGuid(),
            Difficulty = "INVALID",
            Mode = "SINGLE",
            Level = 1,
            MaxScore = 1
        };
        var endpoint = new SongDifficultyEndpoints.Create(_songDifficultyService.Object);

        var result = await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.IsType<BadRequestResult>(result);
        
        _songDifficultyService.Verify(s =>
                s.CreateSongDifficulty(It.IsAny<CreateSongDifficultyRequestModel>(), It.IsAny<CancellationToken>()
                ), Times.Never
        );
    }

    #endregion
}