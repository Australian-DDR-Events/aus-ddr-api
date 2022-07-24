using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Helpers.DataGenerators;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.Infrastructure.Data;

[Collection("Reward repository collection")]
public class RewardRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly RewardRepository _rewardRepository;

    public RewardRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _rewardRepository = new RewardRepository(_fixture._context);
        Setup.DropAllRows(_fixture._context);
        _fixture._context.ChangeTracker.Clear();
    }

    private void AddRewardToTable(Reward d)
    {
        _fixture._context.Rewards.Add(d);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();
    }
    
    #region CreateReward

    [Fact(DisplayName = "When reward is created, add reward to table")]
    public async Task CreateReward_RewardIsCreated()
    {
        var reward = RewardGenerator.CreateReward();

        await _rewardRepository.CreateReward(reward, CancellationToken.None);

        var result = _fixture._context.Rewards.Include(r => r.Triggers).First(r => r.Id.Equals(reward.Id));
        
        Assert.NotNull(reward);
        Assert.Empty(result.Triggers);
    }

    [Fact(DisplayName = "When reward is created with triggers, add reward to table with triggers")]
    public async Task CreateReward_RewardIsCreatedWithTriggers()
    {
        var reward = RewardGenerator.CreateReward();
        reward.Triggers = new List<RewardTrigger>() {new(){Trigger = "trigger1"}, new(){Trigger = "trigger2"}};

        await _rewardRepository.CreateReward(reward, CancellationToken.None);

        var result = _fixture._context.Rewards.Include(r => r.Triggers).First(r => r.Id.Equals(reward.Id));
        
        Assert.NotNull(reward);
        Assert.Equal(2, result.Triggers.Count);
        Assert.Contains(result.Triggers, t => t.Trigger.Equals("trigger1"));
        Assert.Contains(result.Triggers, t => t.Trigger.Equals("trigger2"));
    }

    #endregion

    #region AddTriggerToReward

    [Fact(DisplayName = "When reward exists, does not have trigger, add trigger to reward")]
    public async Task AddTriggerToReward_TriggerNotFound_TriggerAdded()
    {
        var reward = RewardGenerator.CreateReward();
        AddRewardToTable(reward);

        await _rewardRepository.AddTriggerToReward(reward.Id, "trigger", CancellationToken.None);

        var rewardWithTrigger = _fixture._context.Rewards.Include(r => r.Triggers).First(r => r.Id.Equals(reward.Id));
        
        Assert.NotNull(rewardWithTrigger);
        Assert.Single(rewardWithTrigger.Triggers);
        Assert.Equal("trigger", rewardWithTrigger.Triggers.First().Trigger);
    }

    [Fact(DisplayName = "When reward exists, already has trigger, no change")]
    public async Task AddTriggerToReward_RewardHasTrigger_NoChange()
    {
        var reward = RewardGenerator.CreateReward();
        reward.Triggers = new List<RewardTrigger>() {new() {Trigger = "trigger"}};
        AddRewardToTable(reward);

        await _rewardRepository.AddTriggerToReward(reward.Id, "trigger", CancellationToken.None);

        var rewardWithTrigger = _fixture._context.Rewards.Include(r => r.Triggers).First(r => r.Id.Equals(reward.Id));
        
        Assert.NotNull(rewardWithTrigger);
        Assert.Single(rewardWithTrigger.Triggers);
        Assert.Equal("trigger", rewardWithTrigger.Triggers.First().Trigger);
    }

    [Fact(DisplayName = "When reward exists, trigger exists on separate reward, add trigger to both rewards")]
    public async Task AddTriggerToReward_DifferentRewardHasTrigger_AddTriggerToReward()
    {
        var reward = RewardGenerator.CreateReward();
        var secondReward = RewardGenerator.CreateReward();
        reward.Triggers = new List<RewardTrigger>() {new() {Trigger = "trigger"}};
        AddRewardToTable(reward);
        AddRewardToTable(secondReward);

        await _rewardRepository.AddTriggerToReward(secondReward.Id, "trigger", CancellationToken.None);

        var rewardWithTrigger = _fixture._context.Rewards.Include(r => r.Triggers).First(r => r.Id.Equals(reward.Id));
        var secondRewardWithTrigger = _fixture._context.Rewards.Include(r => r.Triggers).First(r => r.Id.Equals(secondReward.Id));
        
        Assert.NotNull(rewardWithTrigger);
        Assert.NotNull(secondRewardWithTrigger);
        Assert.Single(rewardWithTrigger.Triggers);
        Assert.Single(secondRewardWithTrigger.Triggers);
        Assert.Equal("trigger", rewardWithTrigger.Triggers.First().Trigger);
        Assert.Equal("trigger", secondRewardWithTrigger.Triggers.First().Trigger);
    }

    [Fact(DisplayName = "When reward does not exist, do nothing")]
    public async Task AddTriggerToReward_RewardDoesNotExist_Nothing()
    {
        var exception = await Record.ExceptionAsync(async () =>
            await _rewardRepository.AddTriggerToReward(Guid.NewGuid(), "trigger", CancellationToken.None)
        );

        Assert.Null(exception);
    }
    
    #endregion

    #region GetRewardsForTrigger

    [Fact(DisplayName = "When trigger exists, with reward, return corresponding reward")]
    public void GetRewardsForTrigger_TriggerHasRewards_ReturnRewards()
    {
        var reward = RewardGenerator.CreateReward();
        reward.Triggers = new List<RewardTrigger>() {new() {Trigger = "trigger"}};
        AddRewardToTable(reward);

        var rewards = _rewardRepository.GetRewardsForTrigger("trigger").ToList();

        Assert.Single(rewards);
        Assert.Equal(reward.Id, rewards.First().Id);
    }

    [Fact(DisplayName = "When trigger has no corresponding rewards, return empty list")]
    public void GetRewardsForTrigger_TriggerHasNoRewards_ReturnEmpty()
    {
        _fixture._context.RewardTriggers.Add(new RewardTrigger {Trigger = "trigger"});
        _fixture._context.SaveChanges();
        
        var rewards = _rewardRepository.GetRewardsForTrigger("trigger").ToList();
        
        Assert.Empty(rewards);
    }

    [Fact(DisplayName = "When trigger exists, with multiple rewards, return corresponding rewards")]
    public void GetRewardsForTrigger_TriggerHasMultipleRewards_ReturnRewards()
    {
        var reward = RewardGenerator.CreateReward();
        var reward2 = RewardGenerator.CreateReward();
        var trigger = new RewardTrigger
        {
            Trigger = "trigger",
            Rewards = new List<Reward>() {reward, reward2}
        };
        _fixture._context.RewardTriggers.Add(trigger);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();

        var rewards = _rewardRepository.GetRewardsForTrigger("trigger").ToList();

        Assert.Equal(2, rewards.Count);
    }

    #endregion
}