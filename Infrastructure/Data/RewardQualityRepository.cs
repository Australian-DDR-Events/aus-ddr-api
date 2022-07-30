using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
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

    public Task RemoveRewardFromDancer(Guid rewardId, Guid dancerId)
    {
        throw new NotImplementedException();
    }

    public RewardQuality GetRewardQualityForDancer(Guid rewardId, Guid dancerId)
    {
        throw new NotImplementedException();
    }

    public Task CreateRewardQuality(RewardQuality rewardQuality)
    {
        throw new NotImplementedException();
    }
}
