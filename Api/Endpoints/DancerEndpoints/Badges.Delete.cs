using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using AusDdrApi.Attributes;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class Badges_Delete : EndpointWithoutResponse<RevokeBadgeFromDancerByIdRequest>
{
    private readonly IDancerService _dancerService;

    public Badges_Delete(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
        
    [HttpDelete(RevokeBadgeFromDancerByIdRequest.Route)]
    [SwaggerOperation(
        Summary = "Assign a badge to a dancer",
        Description = "Assign an existing badge to the specified dancer",
        OperationId = "Dancers.Badges.Add",
        Tags = new[] { "Dancers", "Badges" })
    ]
    [Authorize]
    [Admin]
    public override async Task<ActionResult> HandleAsync([FromRoute] RevokeBadgeFromDancerByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _dancerService.RemoveBadgeFromDancer(request.DancerId, request.BadgeId, cancellationToken);
        return this.ConvertToActionResult(result);
    }
}