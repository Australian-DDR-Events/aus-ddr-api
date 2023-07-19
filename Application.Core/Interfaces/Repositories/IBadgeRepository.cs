using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Models.Badge;

namespace Application.Core.Interfaces.Repositories;

public interface IBadgeRepository
{
    Task<bool> CreateBadge(Badge chart, CancellationToken cancellationToken);
    Badge? GetBadgeById(Guid id);
    IEnumerable<GetBadgesResponseModel> GetBadges(int skip, int limit);
}