using System;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IBadgeRepository
{
    Badge? GetBadgeById(Guid id);
}