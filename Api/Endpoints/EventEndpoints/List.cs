using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AusDdrApi.Endpoints.EventEndpoints;

public class List : EndpointWithResponse<IList<ListEventResponse>, IList<Event>>
{
    private readonly IEventService _eventService;
    
    public List(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet(ListEventRequest.Route)]
    [SwaggerOperation(
        Summary = "Gets a collection of Events",
        Description = "Gets a collection of events",
        OperationId = "Events.List",
        Tags = new[] { "Events" })
    ]
    public override async Task<ActionResult<IList<ListEventResponse>>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var events = await _eventService.GetEventsAsync(cancellationToken);
        return this.ConvertToActionResult(events);
    }

    public override IList<ListEventResponse> Convert(IList<Event> u)
    {
        return u.Select(e => new ListEventResponse(e.Id, e.Name, e.Description, e.StartDate, e.EndDate)).ToList();
    }
}