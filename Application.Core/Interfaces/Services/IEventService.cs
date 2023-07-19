using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models;

namespace Application.Core.Interfaces.Services;

public interface IEventService
{
    Task<Result<IEnumerable<Event>>> GetEventsAsync(CancellationToken cancellationToken);

    Task<Result<Event>> GetEventAsync(Guid eventId, CancellationToken cancellationToken);
}