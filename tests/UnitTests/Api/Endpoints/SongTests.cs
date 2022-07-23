using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
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
}