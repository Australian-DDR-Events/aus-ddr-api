using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;

namespace Application.Core.Interfaces.Repositories;

public interface IRewardRepository
{
    public IEnumerable<Reward> GetRewardsForTrigger(string trigger);
    public Task CreateReward(Reward reward, CancellationToken cancellationToken);
    public Task AddTriggerToReward(Guid id, string trigger, CancellationToken cancellationToken);
}