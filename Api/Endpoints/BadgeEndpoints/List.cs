using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Application.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.BadgeEndpoints;

[ApiController]
public class List : ControllerBase
{
    private readonly IBadgeService _badgeService;

    public List(IBadgeService badgeService)
    {
        _badgeService = badgeService;
    }
    
    [HttpGet(GetBadgesRequest.Route)]
    [SwaggerOperation(
        Summary = "Gets a collection of Badges",
        Description = "Gets a collection of badges based on paging request",
        OperationId = "Badges.List",
        Tags = new[] { "Badges" })
    ]
    public ActionResult<IEnumerable<GetBadgesResponse>> HandleAsync([FromQuery] GetBadgesRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var badgesResult = _badgeService.GetBadges(request.Page.GetValueOrDefault(0), request.Limit.GetValueOrDefault(20));
        return Ok(badgesResult.Select(GetBadgesResponse.Convert));
    }
}
