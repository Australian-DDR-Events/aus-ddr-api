using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Services;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using EventEndpoints = AusDdrApi.Endpoints.EventEndpoints;

namespace UnitTests.Api.Endpoints;

public class EventTests
{
    private readonly Mock<IEventService> _eventService = new();

    #region List

    [Fact(DisplayName = "When events are returned, then return an Ok response")]
    public async Task WhenEventsReturned_ResponseIsOk()
    {
        var serviceResponse = new List<Event>
        {
            new()
            {
                Id = Guid.NewGuid()
            },
            new()
            {
                Id = Guid.NewGuid()
            }
        };

        _eventService.Setup(es =>
            es.GetEventsAsync(It.IsAny<CancellationToken>())
        ).Returns(Task.FromResult(Result<IEnumerable<Event>>.Success(serviceResponse)));

        var result = await new EventEndpoints.List(_eventService.Object).HandleAsync(CancellationToken.None);
        Assert.IsType<OkObjectResult>(result.Result);
        var convertedResult = result.Result as OkObjectResult;
        Assert.NotNull(convertedResult);
        var data = convertedResult.Value as IEnumerable<EventEndpoints.ListEventResponse>;
        Assert.NotNull(data);
        Assert.Equal(2, data.Count());
    }
    
    #endregion
}