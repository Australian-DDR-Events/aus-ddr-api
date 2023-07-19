using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.EventEndpoints;

public class List : ControllerBase
{
    private readonly IEventService _eventService;
    
    public List(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("/events")]
    [SwaggerOperation(
        Summary = "Gets a collection of Events",
        Description = "Gets a collection of events",
        OperationId = "Events.List",
        Tags = new[] { "Events" })
    ]
    public async Task<ActionResult<IEnumerable<ListEventResponse>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var events = await _eventService.GetEventsAsync(cancellationToken);
        return events.ResultCode switch
        {
            ResultCode.Ok => Ok(events.Value.Value.Select(ListEventResponse.Convert)),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}