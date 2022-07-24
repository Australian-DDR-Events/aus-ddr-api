using System;
using System.Collections.Generic;
using Application.Core.Entities;
using Application.Core.Models.Badge;

namespace Application.Core.Interfaces.Repositories;

public interface IBadgeRepository
{
    Badge? GetBadgeById(Guid id);
    IEnumerable<GetBadgesResponseModel> GetBadges(int skip, int limit);
}