using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Services;
using Moq;
using Xunit;

namespace UnitTests.Core.Services;

public class EventServiceTests
{
    private readonly Mock<IEventRepository> _repository;
    private readonly IEventService _eventService;

    public EventServiceTests()
    {
        _repository = new Mock<IEventRepository>();
        _eventService = new EventService(_repository.Object);
    }

    #region EventService GetEventsAsync

    [Fact(DisplayName = "GetEvents is called on the repository")]
    public async Task WhenGetEventsAsync_ThenCallGetEventsOnRepository()
    {
        var repositoryResponse = new List<Event>()
        {
            new()
            {
                Id = Guid.NewGuid()
            }
        };

        _repository.Setup(r => 
            r.GetEvents()
        ).Returns(repositoryResponse);
        
        var result = await _eventService.GetEventsAsync(new CancellationToken());
        
        Assert.True(result.IsSuccess);
        Assert.Equal(repositoryResponse, result.Value);
        
        _repository.Verify(mock => mock.GetEvents(), Times.Once());
    }

    #endregion
}