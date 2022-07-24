using System;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IRewardQualityRepository
{
    public Task AddRewardToDancer(Guid rewardId, Guid dancerId);
    public Task RemoveRewardFromDancer(Guid rewardId, Guid dancerId);
    public RewardQuality? GetRewardQualityForDancer(Guid rewardId, Guid dancerId);
}