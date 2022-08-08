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

[Collection("Reward quality repository collection")]
public class RewardQualityRepositoryTests
{
    private readonly PostgresDatabaseFixture _fixture;
    private readonly RewardQualityRepository _rewardRepository;
    private readonly DancerRepository _dancerRepository;

    public RewardQualityRepositoryTests(PostgresDatabaseFixture fixture)
    {
        _fixture = fixture;
        _dancerRepository = new DancerRepository(_fixture._context);
        _rewardRepository = new RewardQualityRepository(_fixture._context, _dancerRepository);
        Setup.DropAllRows(_fixture._context);
        _fixture._context.ChangeTracker.Clear();
    }

    private void AddRewardQuality(RewardQuality rewardQuality)
    {
        _fixture._context.RewardQualities.Add(rewardQuality);
        _fixture._context.SaveChanges();
        _fixture._context.ChangeTracker.Clear();
    }

    #region AddRewardToDancer

    [Fact(DisplayName = "When dancer exists, reward exists, dancer does not have reward, reward added to dancer")]
    public async Task AddRewardToDancer_DancerAndRewardExist_DancerAssignedReward()
    {
        var reward = RewardGenerator.CreateReward();
        var dancer = DancerGenerator.CreateDancer();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        _fixture._context.Rewards.Add(reward);
        _fixture._context.Dancers.Add(dancer);
        AddRewardQuality(rewardQuality);

        await _rewardRepository.AddRewardToDancer(rewardQuality.Id, dancer.Id);

        var dancerResult = _fixture._context.Dancers.Include(d => d.RewardQualities).First(d => d.Id.Equals(dancer.Id));
        
        Assert.Single(dancerResult.RewardQualities);
        Assert.Equal(rewardQuality.Id, dancerResult.RewardQualities.First().Id);
    }

    [Fact(DisplayName = "When dancer exists, reward exists, dancer has reward, avoid exception")]
    public async Task AddRewardToDancer_DancerAndRewardExist_DancerHasReward_NoException()
    {
        var reward = RewardGenerator.CreateReward();
        var dancer = DancerGenerator.CreateDancer();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        rewardQuality.Dancers = new List<Dancer> {dancer};
        _fixture._context.Rewards.Add(reward);
        _fixture._context.Dancers.Add(dancer);
        AddRewardQuality(rewardQuality);

        var exception = await Record.ExceptionAsync(async () =>
                await _rewardRepository.AddRewardToDancer(rewardQuality.Id, dancer.Id)
        );

        Assert.Null(exception);
    }

    #endregion

    #region GetRewardQualityForDancer

    [Fact(DisplayName = "When dancer exists, dancer has a reward quality, return corresponding reward quality")]
    public void GetRewardQualityForDancer_DancerExists_HasReward_ReturnRewardQuality()
    {
        var reward = RewardGenerator.CreateReward();
        var dancer = DancerGenerator.CreateDancer();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        rewardQuality.Dancers = new List<Dancer> {dancer};
        _fixture._context.Rewards.Add(reward);
        _fixture._context.Dancers.Add(dancer);
        AddRewardQuality(rewardQuality);

        var result = _rewardRepository.GetRewardQualityForDancer(reward.Id, dancer.Id);
        
        Assert.NotNull(result);
        Assert.Equal(rewardQuality.Id, result.Id);
    }

    [Fact(DisplayName = "When dancer exists, dancer does not have reward quality, return null")]
    public void GetRewardQualityForDancer_DancerExists_DoesNotHaveReward_ReturnNull()
    {
        var reward = RewardGenerator.CreateReward();
        var dancer = DancerGenerator.CreateDancer();
        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);
        _fixture._context.Rewards.Add(reward);
        _fixture._context.Dancers.Add(dancer);
        AddRewardQuality(rewardQuality);

        var result = _rewardRepository.GetRewardQualityForDancer(reward.Id, dancer.Id);
        
        Assert.Null(result);
    }

    #endregion

    #region CreateRewardQuality

    [Fact(DisplayName = "When reward exists, save reward quality")]
    public async Task RewardExists_SaveRewardQuality()
    {
        var reward = RewardGenerator.CreateReward();
        _fixture._context.Rewards.Add(reward);
        await _fixture._context.SaveChangesAsync();

        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);

        var result = await _rewardRepository.CreateRewardQuality(reward.Id, rewardQuality, CancellationToken.None);
        var databaseValue = _fixture._context.RewardQualities.FirstOrDefault(r => r.Id.Equals(rewardQuality.Id));
        
        Assert.True(result);
        
        Assert.NotNull(databaseValue);
        Assert.Equal(rewardQuality.Id, databaseValue.Id);
        Assert.Equal(reward.Id, databaseValue.RewardId);
    }

    [Fact(DisplayName = "When reward does not exist, return false")]
    public async Task RewardDoesNotExist_DoNotSave()
    {
        var reward = RewardGenerator.CreateReward();

        var rewardQuality = RewardQualityGenerator.CreateRewardQuality(reward);

        var result = await _rewardRepository.CreateRewardQuality(reward.Id, rewardQuality, CancellationToken.None);

        Assert.False(result);
    }
    
    #endregion
}