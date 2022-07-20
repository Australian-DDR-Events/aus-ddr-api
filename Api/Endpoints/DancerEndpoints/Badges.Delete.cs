using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using AusDdrApi.Attributes;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

[ApiController]
public class Badges_Delete : ControllerBase
{
    private readonly IDancerService _dancerService;

    public Badges_Delete(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
        
    [HttpDelete("/dancers/{DancerId:guid}/badges/{BadgeId:guid}")]
    [SwaggerOperation(
        Summary = "Revokes a badge from a dancer",
        Description = "Remove an existing badge from the specified dancer",
        OperationId = "Dancers.Badges.Remove",
        Tags = new[] { "Dancers", "Badges" })
    ]
    [Authorize]
    [Admin]
    public async Task<ActionResult> HandleAsync([FromRoute] RevokeBadgeFromDancerByIdRequest request, CancellationToken cancellationToken = new())
    {
        var result = await _dancerService.RemoveBadgeFromDancer(request.DancerId, request.BadgeId, cancellationToken);
        return result ? Ok() : BadRequest();
    }
}