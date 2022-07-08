using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IDancerRepository
{
    IEnumerable<Dancer> GetDancers(int skip, int limit);
    Dancer? GetDancerById(Guid id);
    Dancer? GetDancerByAuthId(string authId);
    Task CreateDancer(Dancer dancer, CancellationToken cancellationToken);
    Task UpdateDancer(Dancer dancer, CancellationToken cancellationToken);
}
