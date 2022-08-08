using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IRewardQualityRepository
{
    public Task AddRewardToDancer(Guid rewardId, Guid dancerId);
    public Task<bool> RemoveRewardFromDancer(Guid rewardId, Guid dancerId, CancellationToken cancellationToken);
    public RewardQuality? GetRewardQualityForDancer(Guid rewardId, Guid dancerId);
    public Task<bool> CreateRewardQuality(Guid rewardId, RewardQuality rewardQuality, CancellationToken cancellationToken);
}