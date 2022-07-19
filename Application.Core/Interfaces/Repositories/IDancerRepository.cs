using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models.Dancer;

namespace Application.Core.Interfaces.Repositories;

public interface IDancerRepository
{
    IEnumerable<Dancer> GetDancers(int skip, int limit);
    Dancer? GetDancerById(Guid id);
    Dancer? GetDancerByAuthId(string authId);
    Task CreateDancer(Dancer dancer, CancellationToken cancellationToken);
    Task UpdateDancer(Dancer dancer, CancellationToken cancellationToken);
    IEnumerable<GetDancerBadgesResponseModel> GetBadgesForDancer(Guid dancerId);
    Task<bool> AddBadgeToDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken);
}
