using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Models.Dancer;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DancerRepository : IDancerRepository
{
    private readonly EFDatabaseContext _context;

    public DancerRepository(EFDatabaseContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Dancer> GetDancers(int skip, int limit)
    {
        return _context
            .Dancers
            .OrderBy(d => d.DdrName)
            .Skip(skip)
            .Take(limit)
            .Select(d => new Dancer
            {
                Id = d.Id,
                DdrName = d.DdrName,
                ProfilePictureTimestamp = d.ProfilePictureTimestamp
            })
            .ToList();
    }

    public Dancer GetDancerById(Guid id)
    {
        return _context
            .Dancers
            .FirstOrDefault(d => d.Id.Equals(id));
    }

    public Dancer GetDancerByAuthId(string id)
    {
        return _context
            .Dancers
            .FirstOrDefault(d => d.AuthenticationId.Equals(id));
    }

    public async Task CreateDancer(Dancer dancer, CancellationToken cancellationToken)
    {
        if (GetDancerByAuthId(dancer.AuthenticationId) != null) return;
        _context
            .Dancers
            .Add(dancer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDancer(Dancer dancer, CancellationToken cancellationToken)
    {
        var dbDancer = GetDancerById(dancer.Id);
        if (dbDancer == null) return;
        _context.Entry(dbDancer).CurrentValues.SetValues(dancer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<GetDancerBadgesResponseModel> GetBadgesForDancer(Guid dancerId)
    {
        return _context
            .Dancers
            .Where(d => d.Id.Equals(dancerId))
            .Select(d => d.Badges.Select(b => new GetDancerBadgesResponseModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    EventName = b.EventId.HasValue ? b.Event!.Name : string.Empty
                })
            ).FirstOrDefault() ?? new List<GetDancerBadgesResponseModel>();
    }

    public async Task<bool> AddBadgeToDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
    {
        var dancer = _context
            .Dancers
            .Include(d => d.Badges)
            .FirstOrDefault(d => d.Id.Equals(dancerId));
        var badge = _context
            .Badges
            .FirstOrDefault(b => b.Id.Equals(badgeId));
        if (dancer == null || badge == null) return false;
        if (dancer.Badges.Any(b => b.Id.Equals(badge.Id))) return false;
        dancer.Badges.Add(badge);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoveBadgeFromDancer(Guid dancerId, Guid badgeId, CancellationToken cancellationToken)
    {
        var dancer = _context
            .Dancers
            .Include(d => d.Badges)
            .FirstOrDefault(d => d.Id.Equals(dancerId));
        var badge = dancer?.Badges.FirstOrDefault(b => b.Id.Equals(badgeId));
        if (badge == null) return false;
        dancer.Badges.Remove(badge);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoveRewardFromDancer(Guid rewardId, Guid dancerId, CancellationToken cancellationToken)
    {
        var dancer = _context
            .Dancers
            .Include(d => d.RewardQualities)
            .FirstOrDefault(d => d.Id.Equals(dancerId));
        var reward = dancer?.RewardQualities.FirstOrDefault(b => b.Id.Equals(rewardId));
        if (reward == null) return false;
        dancer.RewardQualities.Remove(reward);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}