using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Ardalis.Result;

namespace Application.Core.Interfaces.Services;

public interface IEventService
{
    Task<Result<IList<Event>>> GetEventsAsync(CancellationToken cancellationToken);
}