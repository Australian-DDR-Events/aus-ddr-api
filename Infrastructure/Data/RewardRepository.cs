using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class RewardRepository : IRewardRepository
{
    private readonly EFDatabaseContext _context;

    public RewardRepository(EFDatabaseContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Reward> GetRewardsForTrigger(string trigger)
    {
        return _context
            .Rewards
            .Include(r => r.Triggers)
            .Where(r => r.Triggers.Any(t => t.Trigger.Equals(trigger)))
            .Select(r => new Reward
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                TriggerData = r.TriggerData
            })
            .ToList();
    }

    public async Task CreateReward(Reward reward, CancellationToken cancellationToken)
    {
        _context
            .Rewards
            .Add(reward);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddTriggerToReward(Guid id, string trigger, CancellationToken cancellationToken)
    {
        var reward = _context.Rewards.Include(r => r.Triggers).FirstOrDefault(r => r.Id.Equals(id));
        if (reward == null || reward.Triggers.Any(t => t.Trigger.Equals(trigger))) return;
        var triggerEntry = _context.RewardTriggers.FirstOrDefault(t => t.Trigger.Equals(trigger)) ??
                      new RewardTrigger() {Trigger = trigger};
        reward.Triggers.Add(triggerEntry);
        await _context.SaveChangesAsync(cancellationToken);
    }
}