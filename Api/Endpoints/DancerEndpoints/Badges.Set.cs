using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

[ApiController]
public class Badges_Set : ControllerBase
{
    private readonly IDancerService _dancerService;

    public Badges_Set(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
        
    [HttpPost("/dancers/{DancerId:guid}/badges/{BadgeId:guid}")]
    [SwaggerOperation(
        Summary = "Assign a badge to a dancer",
        Description = "Assign an existing badge to the specified dancer",
        OperationId = "Dancers.Badges.Add",
        Tags = new[] { "Dancers", "Badges" })
    ]
    [Authorize]
    [Admin]
    public async Task<ActionResult> HandleAsync([FromRoute] AddBadgeToDancerByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await _dancerService.AddBadgeToDancer(request.DancerId, request.BadgeId, cancellationToken);

        return result.ResultCode switch
        {
            ResultCode.Ok => Ok(),
            ResultCode.NotFound => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError),
        };
    }
}
