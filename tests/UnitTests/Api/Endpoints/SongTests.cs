using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Application.Core.Models.Song;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Moq;
using Xunit;
using SongEndpoints = AusDdrApi.Endpoints.SongEndpoints;

namespace UnitTests.Api.Endpoints;

public class SongTests
{
    private readonly Mock<ISongService> _songService = new();

    #region List

    [Fact(DisplayName = "Convert song response into response payload")]
    public void List_SongResultFromService_ConvertToResponse()
    {
        var request = new SongEndpoints.GetAllSongsRequest
        {
            Limit = 5,
            Page = 3
        };
        var serviceResponse = new List<Song>
        {
            new Song
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Artist = "Artist"
            }
        };

        _songService.Setup(s =>
            s.GetSongs(It.IsAny<int>(), It.IsAny<int>())
        ).Returns(serviceResponse);

        var response = new SongEndpoints.List(_songService.Object).HandleAsync(request);

        Assert.IsType<OkObjectResult>(response.Result);
        var convertedResult = response.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as IEnumerable<SongEndpoints.GetAllSongsResponse>;
        Assert.NotNull(data);
        Assert.Single(data);
    }

    #endregion

    #region GetById

    [Fact(DisplayName = "When song found, return song result")]
    public void GetById_SongFound_ReturnsSong()
    {
        var request = new SongEndpoints.GetSongByIdRequest
        {
            Id = Guid.NewGuid()
        };
        var song = new Song
        {
            Id = request.Id,
            Name = "Name",
            Artist = "Artist",
            Charts = new List<Chart>()
        };

        _songService.Setup(s =>
            s.GetSong(It.IsAny<Guid>())
        ).Returns(new Result<Song>
        {
            ResultCode = ResultCode.Ok,
            Value = song
        });

        var response = new SongEndpoints.GetById(_songService.Object).HandleAsync(request, CancellationToken.None);

        Assert.IsType<OkObjectResult>(response.Result);
        var convertedResult = response.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as SongEndpoints.GetSongByIdResponse;
        Assert.NotNull(data);
        Assert.Equal(request.Id, data.Id);
        
        _songService.Verify(s =>
            s.GetSong(It.Is<Guid>(value => value.Equals(request.Id))),
            Times.Once);
    }

    [Fact(DisplayName = "When song not found, return not found result")]
    public void GetById_SongNotFound_NotFoundResult()
    {
        var request = new SongEndpoints.GetSongByIdRequest
        {
            Id = Guid.NewGuid()
        };

        _songService.Setup(s =>
            s.GetSong(It.IsAny<Guid>())
        ).Returns(new Result<Song>
        {
            ResultCode = ResultCode.NotFound,
            Value = new Optional<Song>()
        });

        var response = new SongEndpoints.GetById(_songService.Object).HandleAsync(request, CancellationToken.None);

        Assert.IsType<NotFoundResult>(response.Result);
    }
    
    #endregion

    #region Create

    [Fact(DisplayName = "When create, service is called with model")]
    public async Task Create_SongIsCreated()
    {
        var request = new SongEndpoints.CreateSongRequest
        {
            Name = "Name",
            Artist = "Artist",
            KonamiId = "abc"
        };

        var response = await new SongEndpoints.Create(_songService.Object).HandleAsync(request, CancellationToken.None);

        Assert.IsType<AcceptedResult>(response);
        
        _songService.Verify(s =>
            s.CreateSongAsync(
                It.Is<CreateSongRequestModel>(value => 
                    value.Name.Equals(request.Name) &&
                    value.Artist.Equals(request.Artist) &&
                    value.KonamiId.Equals(request.KonamiId)),
                CancellationToken.None),
            Times.Once);
    }

    #endregion
}