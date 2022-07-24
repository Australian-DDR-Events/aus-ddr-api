using System;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;

namespace Infrastructure.Data;

public class RewardQualityRepository : IRewardQualityRepository
{
    private readonly EFDatabaseContext _context;

    public RewardQualityRepository(EFDatabaseContext context)
    {
        _context = context;
    }

    public Task AddRewardToDancer(Guid rewardId, Guid dancerId)
    {
        throw new NotImplementedException();
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