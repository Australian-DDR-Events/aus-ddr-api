using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class Badges_List : ControllerBase
{
    private readonly IDancerService _dancerService;

    public Badges_List(IDancerService dancerService)
    {
        _dancerService = dancerService;
    }
        
    [HttpGet("/dancers/{Id:guid}/badges")]
    [SwaggerOperation(
        Summary = "Gets a collection of badges for a dancer",
        Description = "Gets all badges a given dancer has unlocked",
        OperationId = "Dancers.Badges.List",
        Tags = new[] { "Dancers", "Badges" })
    ]
    public async Task<ActionResult<IEnumerable<GetDancerBadgesByIdResponse>>> HandleAsync([FromRoute] GetDancerBadgesByIdRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var badgesResult = _dancerService.GetDancerBadges(request.Id);
        if (badgesResult.IsSuccess) return Ok(badgesResult.Value.Select(GetDancerBadgesByIdResponse.Convert));
        if (badgesResult.Status == ResultStatus.NotFound) return NotFound();
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}
