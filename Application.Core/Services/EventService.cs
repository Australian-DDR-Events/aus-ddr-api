using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Services;
using Ardalis.Result;

namespace Application.Core.Services;

public class EventService : CommonService<Event>, IEventService
{
    private readonly IAsyncRepository<Event> _eventRepository;

    public EventService(IAsyncRepository<Event> eventRepository) : base(eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<Result<IList<Event>>> GetEventsAsync(CancellationToken cancellationToken)
    {
        return Result<IList<Event>>.Success(await _eventRepository.ListAsync(cancellationToken));
    }
}