using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class RewardQualityRepository : IRewardQualityRepository
{
    private readonly EFDatabaseContext _context;
    private readonly IDancerRepository _dancerRepository;

    public RewardQualityRepository(EFDatabaseContext context, IDancerRepository dancerRepository)
    {
        _context = context;
        _dancerRepository = dancerRepository;
    }

    public async Task AddRewardToDancer(Guid rewardId, Guid dancerId)
    {
        var dancer = _dancerRepository.GetDancerById(dancerId);
        if (dancer == null) return;
        var reward = await _context.RewardQualities.Include(r => r.Dancers).FirstOrDefaultAsync(r => r.Id.Equals(rewardId));
        if (reward == null || reward.Dancers.Any(d => d.Id.Equals(dancerId))) return;
        reward.Dancers.Add(dancer);
        await _context.SaveChangesAsync();
    }

    public Task<bool> RemoveRewardFromDancer(Guid rewardId, Guid dancerId, CancellationToken cancellationToken)
    {
        return _dancerRepository.RemoveRewardFromDancer(rewardId, dancerId, cancellationToken);
    }

    public RewardQuality GetRewardQualityForDancer(Guid rewardId, Guid dancerId)
    {
        return _context
            .RewardQualities
            .FirstOrDefault(r => 
                r.RewardId.Equals(rewardId) && 
                r.Dancers.Any(d => d.Id.Equals(dancerId))
        );
    }

    public async Task<bool> CreateRewardQuality(Guid rewardId, RewardQuality rewardQuality, CancellationToken cancellationToken)
    {
        var reward = _context.Rewards.FirstOrDefault(r => r.Id.Equals(rewardId));
        if (reward == null) return false;
        rewardQuality.RewardId = rewardId;
        _context.RewardQualities.Add(rewardQuality);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
