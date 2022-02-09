using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class Badges_List : EndpointWithResponse<GetDancerBadgesByIdRequest, IEnumerable<GetDancerBadgesByIdResponse>, ICollection<Badge>>
{
    private readonly IDancerService _dancerService;

    public Badges_List(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
        
    [HttpGet(GetDancerBadgesByIdRequest.Route)]
    [SwaggerOperation(
        Summary = "Gets a collection of badges for a dancer",
        Description = "Gets all badges a given dancer has unlocked",
        OperationId = "Dancers.Badges.List",
        Tags = new[] { "Dancers", "Badges" })
    ]
    public override async Task<ActionResult<IEnumerable<GetDancerBadgesByIdResponse>>> HandleAsync([FromRoute] GetDancerBadgesByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var dancersResult = await _dancerService.GetDancerBadgesAsync(request.Id, cancellationToken);
        return this.ConvertToActionResult(dancersResult);
    }

    public override IEnumerable<GetDancerBadgesByIdResponse> Convert(ICollection<Badge> u)
    {
        return u.Select(b => new GetDancerBadgesByIdResponse(b.Id, b.Name, b.Description, b.Event?.Name ?? ""));
    }
}