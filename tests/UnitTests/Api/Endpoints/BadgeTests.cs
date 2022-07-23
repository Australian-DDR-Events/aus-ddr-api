using System.Collections.Generic;
using Application.Core.Interfaces.Services;
using Application.Core.Models.Badge;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace UnitTests.Api.Endpoints;

using BadgeEndpoints = AusDdrApi.Endpoints.BadgeEndpoints;

public class BadgeTests
{
    private readonly Mock<IBadgeService> _badgeService = new();
    
    #region List

    [Fact(DisplayName = "When badges fetched from service, result is OkResult")]
    public void List_BadgesFetched_OkResultReturned()
    {
        var request = new BadgeEndpoints.GetBadgesRequest
        {
            Limit = 5,
            Page = 10
        };

        _badgeService.Setup(s =>
            s.GetBadges(It.IsAny<int>(), It.IsAny<int>())
        ).Returns(new List<GetBadgesResponseModel> {new GetBadgesResponseModel()}); 

        var result = new BadgeEndpoints.List(_badgeService.Object).HandleAsync(request);
        
        Assert.IsType<OkObjectResult>(result.Result);
        var convertedResult = result.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as IEnumerable<BadgeEndpoints.GetBadgesResponse>;
        Assert.NotNull(data);
        Assert.Single(data);
    }

    #endregion
}