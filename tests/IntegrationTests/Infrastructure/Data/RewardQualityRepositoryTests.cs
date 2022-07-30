using System.Collections.Generic;
using System.Linq;
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
}