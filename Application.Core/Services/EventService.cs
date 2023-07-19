using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Models;
using Microsoft.CodeAnalysis;

namespace Application.Core.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Event>>> GetEventsAsync(CancellationToken cancellationToken)
    {
        return new Result<IEnumerable<Event>>
        {
            ResultCode = ResultCode.Ok,
            Value = new Optional<IEnumerable<Event>>(_repository.GetEvents())
        };
    }

    public Task<Result<Event>> GetEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var result = _repository.GetEventWithTopScore(eventId);

        return Task.FromResult(result == null
            ? new Result<Event>
            {
                ResultCode = ResultCode.NotFound,
                Value = new Optional<Event>()
            }
            : new Result<Event>
            {
                ResultCode = ResultCode.Ok,
                Value = result
            }
        );
    }
}