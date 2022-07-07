using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Application.Core.Models.Dancer;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DancerEndpoints = AusDdrApi.Endpoints.DancerEndpoints;

namespace UnitTests.Api.Endpoints;

public class DancerTests
{
    private readonly Mock<IDancerService> _dancerService = new();
    private readonly Mock<IIdentity<string>> _identityService = new();

    #region GetByToken

    [Fact(DisplayName = "When dancer is returned from service, return ok result")]
    public async Task GetByToken_WhenDancerReturned_ResponseIsOk()
    {
        var endpoint = new DancerEndpoints.GetByToken(_dancerService.Object, _identityService.Object);

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };

        var dancer = new Dancer
        {
            Id = Guid.NewGuid()
        };
        
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(ds =>
            ds.MigrateDancer(It.Is<MigrateDancerRequestModel>(source =>
                source.LegacyId == identityResponse.LegacyId &&
                source.AuthId == identityResponse.UserId
            ), It.IsAny<CancellationToken>())
        ).ReturnsAsync(Result<Dancer>.Success(dancer));

        var response = await endpoint.HandleAsync("", CancellationToken.None);
        
        Assert.IsType<OkObjectResult>(response.Result);
        var convertedResult = response.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as DancerEndpoints.GetDancerByTokenResponse;
        Assert.NotNull(data);
        Assert.Equal(dancer.Id, data.Id);
    }

    [Fact(DisplayName = "When dancer is not found, return NotFound result")]
    public async Task GetByToken_DancerNotFound_ResponseIsNotFound()
    {
        var endpoint = new DancerEndpoints.GetByToken(_dancerService.Object, _identityService.Object);
        
        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(ds =>
            ds.MigrateDancer(It.IsAny<MigrateDancerRequestModel>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(Result<Dancer>.NotFound());

        var response = await endpoint.HandleAsync("", CancellationToken.None);

        Assert.IsType<NotFoundResult>(response.Result);
    }

    #endregion
}
