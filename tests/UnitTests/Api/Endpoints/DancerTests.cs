using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Application.Core.Models.Dancer;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DancerEndpoints = AusDdrApi.Endpoints.DancerEndpoints;

namespace UnitTests.Api.Endpoints;

public class DancerTests
{
    private readonly Mock<IDancerService> _dancerService = new();
    private readonly Mock<IIdentity<string>> _identityService = new();

    #region GetById

    
    [Fact(DisplayName = "When dancer is returned from service, return ok result")]
    public async Task GetById_WhenDancerReturned_ResponseIsOk()
    {
        var endpoint = new DancerEndpoints.GetById(_dancerService.Object);

        var request = new DancerEndpoints.GetDancerByIdRequest
        {
            Id = Guid.NewGuid()
        };
        var dancer = new Dancer
        {
            Id = request.Id
        };

        _dancerService.Setup(ds =>
            ds.GetDancerById(It.Is<Guid>(source => source.Equals(request.Id)
            ))
        ).Returns(Result<Dancer>.Success(dancer));

        var response = await endpoint.HandleAsync(request, CancellationToken.None);
        
        Assert.IsType<OkObjectResult>(response.Result);
        var convertedResult = response.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as DancerEndpoints.GetDancerByIdResponse;
        Assert.NotNull(data);
        Assert.Equal(dancer.Id, data.Id);
    }

    [Fact(DisplayName = "When dancer is not found, return NotFound result")]
    public async Task GetById_DancerNotFound_ResponseIsNotFound()
    {
        var endpoint = new DancerEndpoints.GetById(_dancerService.Object);
        
        var request = new DancerEndpoints.GetDancerByIdRequest
        {
            Id = Guid.NewGuid()
        };

        _dancerService.Setup(ds =>
            ds.GetDancerById(It.IsAny<Guid>())
        ).Returns(Result<Dancer>.NotFound());

        var response = await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.IsType<NotFoundResult>(response.Result);
    }

    #endregion

    #region GetByToken

    [Fact(DisplayName = "When dancer is returned from service, return ok result")]
    public async Task GetByToken_WhenDancerReturned_ResponseIsOk()
    {
        var endpoint = new DancerEndpoints.GetByToken(_dancerService.Object, _identityService.Object)
         {
             ControllerContext =
             {
                 HttpContext = new DefaultHttpContext()
             }
         };
        endpoint.ControllerContext.HttpContext.Items = new Dictionary<object, object>();
        endpoint.ControllerContext.HttpContext.Items.Add("cookie", "cookie");

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

        var response = await endpoint.HandleAsync(CancellationToken.None);
        
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
        var endpoint = new DancerEndpoints.GetByToken(_dancerService.Object, _identityService.Object)
        {
            ControllerContext =
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        endpoint.ControllerContext.HttpContext.Items = new Dictionary<object, object>();
        endpoint.ControllerContext.HttpContext.Items.Add("cookie", "cookie");

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

        var response = await endpoint.HandleAsync(CancellationToken.None);

        Assert.IsType<NotFoundResult>(response.Result);
    }

    #endregion

    #region Update

    [Fact(DisplayName = "When dancer updates, return Accepted result")]
    public async Task Update_DancerUpdates_ResponseIsAccepted()
    {
        var endpoint = new DancerEndpoints.Update(_dancerService.Object, _identityService.Object);

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        var requestModel = new DancerEndpoints.UpdateDancerByAuthIdRequest
        {
            DdrCode = "123",
            DdrName = "abc",
            PrimaryMachineLocation = "zyx",
            State = "xyz"
        };
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(d =>
            d.UpdateDancerAsync(It.Is<UpdateDancerRequestModel>(value =>
                value.AuthId == identityResponse.UserId), It.IsAny<CancellationToken>())
        ).ReturnsAsync(Result<Dancer>.Success(new Dancer {Id = Guid.NewGuid()}));

        var response = await endpoint.HandleAsync(requestModel, "", CancellationToken.None);
        Assert.IsType<AcceptedResult>(response);
    }

    [Fact(DisplayName = "When dancer does not update, return not found result")]
    public async Task Update_DancerNotFound_ResponseIsNotFound()
    {
        var endpoint = new DancerEndpoints.Update(_dancerService.Object, _identityService.Object);

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        var requestModel = new DancerEndpoints.UpdateDancerByAuthIdRequest
        {
            DdrCode = "123",
            DdrName = "abc",
            PrimaryMachineLocation = "zyx",
            State = "xyz"
        };
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(d =>
            d.UpdateDancerAsync(It.IsAny<UpdateDancerRequestModel>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(Result<Dancer>.NotFound());

        var response = await endpoint.HandleAsync(requestModel, "", CancellationToken.None);
        Assert.IsType<NotFoundResult>(response);
    }

    #endregion

    #region Create

    [Fact(DisplayName = "When dancer is created, return Accepted result")]
    public async Task Create_DancerCreated_ResponseIsAccepted()
    {
        var endpoint = new DancerEndpoints.Create(_dancerService.Object, _identityService.Object);

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        var requestModel = new DancerEndpoints.CreateDancerByAuthIdRequest
        {
            DdrCode = "123",
            DdrName = "abc",
            PrimaryMachineLocation = "zyx",
            State = "xyz"
        };

        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(d =>
            d.CreateDancerAsync(It.Is<CreateDancerRequestModel>(value =>
                value.AuthId == identityResponse.UserId), It.IsAny<CancellationToken>())
        ).ReturnsAsync(Result<Dancer>.Success(new Dancer {Id = Guid.NewGuid()}));

        var response = await endpoint.HandleAsync(requestModel, "", CancellationToken.None);
        Assert.IsType<AcceptedResult>(response);
    }

    [Fact(DisplayName = "When dancer already exists, return Conflict result")]
    public async Task Create_DancerExists_ResponseIsConflict()
    {
        var endpoint = new DancerEndpoints.Create(_dancerService.Object, _identityService.Object);

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        var requestModel = new DancerEndpoints.CreateDancerByAuthIdRequest
        {
            DdrCode = "123",
            DdrName = "abc",
            PrimaryMachineLocation = "zyx",
            State = "xyz"
        };

        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(d =>
            d.CreateDancerAsync(It.Is<CreateDancerRequestModel>(value =>
                value.AuthId == identityResponse.UserId), It.IsAny<CancellationToken>())
        ).ReturnsAsync(Result<Dancer>.Error("error"));

        var response = await endpoint.HandleAsync(requestModel, "", CancellationToken.None);
        Assert.IsType<ConflictResult>(response);
    }

    #endregion

    #region Badges_List

    [Fact(DisplayName = "When success response with badges, return Ok result")]
    public async Task Badges_List_BadgesFound_OkResponse()
    {
        var endpoint = new DancerEndpoints.Badges_List(_dancerService.Object);

        var request = new DancerEndpoints.GetDancerBadgesByIdRequest
        {
            Id = Guid.NewGuid()
        };

        var serviceResponse = new List<GetDancerBadgesResponseModel>()
        {
            new()
        };

        _dancerService.Setup(ds =>
            ds.GetDancerBadges(It.Is<Guid>(value => value.Equals(request.Id)))
        ).Returns(serviceResponse);

        var response = await endpoint.HandleAsync(request);

        Assert.IsType<OkObjectResult>(response.Result);
        var convertedResult = response.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as IEnumerable<DancerEndpoints.GetDancerBadgesByIdResponse>;
        Assert.NotNull(data);
        Assert.Single(data);
    }

    #endregion

    #region Badges_Set

    [Fact(DisplayName = "When success response, return Ok result")]
    public async Task Badges_Set_Assigned_OkResponse()
    {
        var endpoint = new DancerEndpoints.Badges_Set(_dancerService.Object);
        var request = new DancerEndpoints.AddBadgeToDancerByIdRequest()
        {
            BadgeId = Guid.NewGuid(),
            DancerId = Guid.NewGuid()
        };

        _dancerService.Setup(ds =>
            ds.AddBadgeToDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        var response = await endpoint.HandleAsync(request);
        
        Assert.IsType<OkResult>(response);
        _dancerService.Verify(ds =>
                ds.AddBadgeToDancer(
                    It.Is<Guid>(value => value.Equals(request.DancerId)),
                    It.Is<Guid>(value => value.Equals(request.BadgeId)),
                    It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Fact(DisplayName = "When error response, return BadRequest result")]
    public async Task Badges_Set_Failed_BadRequestResponse()
    {
        var endpoint = new DancerEndpoints.Badges_Set(_dancerService.Object);
        var request = new DancerEndpoints.AddBadgeToDancerByIdRequest()
        {
            BadgeId = Guid.NewGuid(),
            DancerId = Guid.NewGuid()
        };

        _dancerService.Setup(ds =>
            ds.AddBadgeToDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var response = await endpoint.HandleAsync(request);
        
        Assert.IsType<BadRequestResult>(response);
    }

    #endregion

    #region Badges_Delete

    [Fact(DisplayName = "When success response, return Ok result")]
    public async Task Badges_Delete_Assigned_OkResponse()
    {
        var endpoint = new DancerEndpoints.Badges_Delete(_dancerService.Object);
        var request = new DancerEndpoints.RevokeBadgeFromDancerByIdRequest()
        {
            BadgeId = Guid.NewGuid(),
            DancerId = Guid.NewGuid()
        };

        _dancerService.Setup(ds =>
            ds.RemoveBadgeFromDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        var response = await endpoint.HandleAsync(request);
        
        Assert.IsType<OkResult>(response);
        _dancerService.Verify(ds =>
                ds.RemoveBadgeFromDancer(
                    It.Is<Guid>(value => value.Equals(request.DancerId)),
                    It.Is<Guid>(value => value.Equals(request.BadgeId)),
                    It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Fact(DisplayName = "When error response, return BadRequest result")]
    public async Task Badges_Delete_Failed_BadRequestResponse()
    {
        var endpoint = new DancerEndpoints.Badges_Delete(_dancerService.Object);
        var request = new DancerEndpoints.RevokeBadgeFromDancerByIdRequest()
        {
            BadgeId = Guid.NewGuid(),
            DancerId = Guid.NewGuid()
        };

        _dancerService.Setup(ds =>
            ds.RemoveBadgeFromDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var response = await endpoint.HandleAsync(request);
        
        Assert.IsType<BadRequestResult>(response);
    }

    #endregion

    #region Avatar_Set

    

    [Fact(DisplayName = "When success response, return Ok result")]
    public async Task Avatar_Set_Success_OkResponse()
    {
        var endpoint = new DancerEndpoints.Avatar_Set(_dancerService.Object, _identityService.Object);
        var request = new DancerEndpoints.SetAvatarForDancerByTokenRequest
        {
            Image = new FormFile(new MemoryStream(), 0, 0, "name", "fileName")
        };

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(ds =>
            ds.SetAvatarForDancerByAuthId(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(true);

        var response = await endpoint.HandleAsync(request, "", CancellationToken.None);
        
        Assert.IsType<OkResult>(response);
        _dancerService.Verify(ds =>
                ds.SetAvatarForDancerByAuthId(
                    It.Is<string>(value => value.Equals(identityResponse.UserId)),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Fact(DisplayName = "When error response, return BadRequest result")]
    public async Task Avatar_Set_Failure_NotFound()
    {
        var endpoint = new DancerEndpoints.Avatar_Set(_dancerService.Object, _identityService.Object);
        var request = new DancerEndpoints.SetAvatarForDancerByTokenRequest
        {
            Image = new FormFile(new MemoryStream(), 0, 0, "name", "fileName")
        };

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);

        _dancerService.Setup(ds =>
            ds.SetAvatarForDancerByAuthId(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(false);

        var response = await endpoint.HandleAsync(request, "", CancellationToken.None);
        
        Assert.IsType<NotFoundResult>(response);
        _dancerService.Verify(ds =>
                ds.SetAvatarForDancerByAuthId(
                    It.Is<string>(value => value.Equals(identityResponse.UserId)),
                    It.IsAny<Stream>(),
                    It.IsAny<CancellationToken>()
                ), Times.Once
        );
    }

    [Fact(DisplayName = "When image is null, return bad request")]
    public async Task Avatar_Set_NullImage_BadRequest()
    {
        var endpoint = new DancerEndpoints.Avatar_Set(_dancerService.Object, _identityService.Object);
        var request = new DancerEndpoints.SetAvatarForDancerByTokenRequest
        {
            Image = null
        };

        var identityResponse = new UserInfo
        {
            LegacyId = Guid.NewGuid().ToString(),
            UserId = Guid.NewGuid().ToString()
        };
        
        _identityService.Setup(i =>
            i.GetUserInfo(It.IsAny<string>())
        ).ReturnsAsync(identityResponse);
        
        var response = await endpoint.HandleAsync(request, "", CancellationToken.None);

        Assert.IsType<BadRequestResult>(response);
    }

    #endregion
}
