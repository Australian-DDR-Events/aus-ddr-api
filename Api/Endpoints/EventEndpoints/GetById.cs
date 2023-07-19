using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Microsoft.AspNetCore.Http;
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
        return result.ResultCode switch
        {
            ResultCode.Ok => GetEventByIdResponse.Convert(result.Value.Value),
            ResultCode.NotFound => NotFound(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
