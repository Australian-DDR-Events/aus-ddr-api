using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.BadgeEndpoints;

[ApiController]
public class Create : ControllerBase
{
    private readonly IBadgeService _badgeService;

    public Create(IBadgeService badgeService)
    {
        _badgeService = badgeService;
    }

    [HttpPost(CreateBadgeRequest.Route)]
    [SwaggerOperation(
        Summary = "Add a badge song",
        Description = "",
        OperationId = "",
        Tags = new[] { "Song" })
    ]
    [Authorize]
    [Admin]
    public async Task<ActionResult<CreateBadgeResponse>> HandleAsync(CreateBadgeRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var entity = new Badge
        {
            Name = request.Name,
            Description = request.Description,
            Threshold = request.Threshold,
            EventId = request.EventId
        };
        var createdBadge = await _badgeService.CreateBadgeAsync(entity, cancellationToken);
        return createdBadge.Value.HasValue switch
        {
            true => Created("", Convert(createdBadge.Value.Value)),
            false => BadRequest()
        };
    }

    private CreateBadgeResponse Convert(Badge u)
    {
        return new CreateBadgeResponse
        {
            Id = u.Id,
            Name = u.Name,
            Description = u.Description
        };
    }
}
