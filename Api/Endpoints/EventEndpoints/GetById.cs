using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.EventEndpoints;

[ApiController]
public class GetById : ControllerBase
{
    private readonly IEventService _eventService;

    public GetById(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("/events/{Id:guid}")]
    [SwaggerOperation(
        Summary = "Gets a detailed listing of an event",
        Description = "Gets an event with all songs and courses",
        OperationId = "Events.GetById",
        Tags = new[] { "Events" })
    ]
    public async Task<ActionResult<GetEventByIdResponse>> HandleAsync([FromRoute] GetEventByIdRequest request, CancellationToken cancellationToken = new())
    {
        var result = await _eventService.GetEventAsync(request.Id, cancellationToken);
        if (result.IsSuccess) return Ok(GetEventByIdResponse.Convert(result.Value));
        return NotFound();
    }
}