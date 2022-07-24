using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Models.Badge;
using Ardalis.Specification.EntityFrameworkCore;
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
    
    public IEnumerable<GetBadgesResponseModel> GetBadges(int skip, int limit)
    {
        return _context
            .Badges
            .Skip(skip)
            .Take(limit)
            .Select(s => new GetBadgesResponseModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                EventName = s.Event == null ? null : s.Event.Name
            })
            .ToList();
    }
}