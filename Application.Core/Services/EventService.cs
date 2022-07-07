using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Ardalis.Result;

namespace Application.Core.Services;

public class EventService : CommonService<Event>, IEventService
{
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IEventRepository _repository;

    public EventService(IAsyncRepository<Event> eventRepository, IEventRepository repository) : base(eventRepository)
    {
        _eventRepository = eventRepository;
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Event>>> GetEventsAsync(CancellationToken cancellationToken)
    {
        return Result<IEnumerable<Event>>.Success(_repository.GetEvents());
    }

    public Task<Result<Event>> GetEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var result = _repository.GetEventWithTopScore(eventId);
        return Task.FromResult(result == null ? Result<Event>.NotFound() : Result<Event>.Success(result));
    }
}