using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Application.Core.Services;
using Moq;
using Xunit;

namespace UnitTests.Core.Services;

public class EventServiceTests
{
    private readonly Mock<IAsyncRepository<Event>> _eventRepository;
    private readonly IEventService _eventService;

    public EventServiceTests()
    {
        _eventRepository = new Mock<IAsyncRepository<Event>>();
        _eventService = new EventService(_eventRepository.Object);
    }

    #region EventService GetEventsAsync

    [Fact(DisplayName = "ListAsync is called on the Event Repository")]
    public async Task When_EventServiceGetEventsAsync_Then_ListAsyncCalledOnRepository()
    {
        var repositoryResponse = new List<Event>()
        {
            new()
            {
                Id = Guid.NewGuid()
            }
        };
        _eventRepository.Setup(r =>
            r.ListAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(repositoryResponse));

        var result = await _eventService.GetEventsAsync(new CancellationToken());
        
        Assert.True(result.IsSuccess);
        Assert.Equal(repositoryResponse, result.Value);
    }

    #endregion
}