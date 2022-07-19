using System;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using JetBrains.Annotations;

namespace Infrastructure.Data;

public class BadgeRepository : IBadgeRepository
{
    private readonly EFDatabaseContext _context;

    public BadgeRepository(EFDatabaseContext context)
    {
        _context = context;
    }
    
    [CanBeNull]
    public Badge GetBadgeById(Guid id)
    {
        return _context
            .Badges
            .FirstOrDefault(b => b.Id.Equals(id));
    }
}