using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using AusDdrApi.Attributes;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class Badges_Set : EndpointWithoutResponse<AddBadgeToDancerByIdRequest>
{
    private readonly IDancerService _dancerService;

    public Badges_Set(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
        
    [HttpPost(AddBadgeToDancerByIdRequest.Route)]
    [SwaggerOperation(
        Summary = "Assign a badge to a dancer",
        Description = "Assign an existing badge to the specified dancer",
        OperationId = "Dancers.Badges.Add",
        Tags = new[] { "Dancers", "Badges" })
    ]
    [Authorize]
    [Admin]
    public override async Task<ActionResult> HandleAsync([FromRoute] AddBadgeToDancerByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _dancerService.AddBadgeToDancer(request.DancerId, request.BadgeId, cancellationToken);
        return this.ConvertToActionResult(result);
    }
}