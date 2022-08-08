using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Entities;
using Application.Core.Interfaces.Repositories;
using Application.Core.Models.Strategies;
using Application.Core.Services.RewardStrategies;
using Moq;
using Xunit;

namespace UnitTests.Core.Services.RewardStrategies;

public class SongScoreStrategyTests
{
    private readonly SongScoreStrategy _songScoreStrategy;
    private readonly Mock<IRewardQualityRepository> _rewardQualityRepository;

    private static List<SongScoreStrategyModel.ScoreRewardThreshold> THRESHOLDS =
        new()
        {
            new SongScoreStrategyModel.ScoreRewardThreshold
            {
                RewardItemId = Guid.NewGuid(),
                MinExScore = 10
            },
            new SongScoreStrategyModel.ScoreRewardThreshold
            {
                RewardItemId = Guid.NewGuid(),
                MinExScore = 50
            },
            new SongScoreStrategyModel.ScoreRewardThreshold
            {
                RewardItemId = Guid.NewGuid(),
                MinExScore = 100
            },
        };

    public SongScoreStrategyTests()
    {
        _rewardQualityRepository = new Mock<IRewardQualityRepository>();
        _songScoreStrategy = new SongScoreStrategy(_rewardQualityRepository.Object);
    }

    [Fact(DisplayName = "When dancer has no reward, dancer qualifies for reward, assign reward to dancer")]
    public async Task Execute_DancerHasNoReward_DancerQualifies_RewardAssigned()
    {
        var reward = Guid.NewGuid();
        var score = new Score
        {
            Id = Guid.NewGuid(),
            DancerId = Guid.NewGuid(),
            ExScore = 60
        };
        var storedData = new SongScoreStrategyModel.StoredData()
        {
            Rewards = THRESHOLDS
        };
        var strategyModel = new SongScoreStrategyModel
        {
            Score = score,
            Data = storedData
        };

        _rewardQualityRepository.Setup(r =>
            r.GetRewardQualityForDancer(It.IsAny<Guid>(), It.IsAny<Guid>())
        ).Returns(null as RewardQuality);

        var expectedReward = THRESHOLDS.First(r => r.MinExScore.Equals(50));

        await _songScoreStrategy.Execute(reward, strategyModel, CancellationToken.None);

        _rewardQualityRepository.Verify(r =>
                r.GetRewardQualityForDancer(
                    It.Is<Guid>(value => value.Equals(reward)),
                    It.Is<Guid>(value => value.Equals(score.DancerId))),
            Times.Once());
        _rewardQualityRepository.Verify(r =>
                r.RemoveRewardFromDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _rewardQualityRepository.Verify(r =>
            r.AddRewardToDancer(
                It.Is<Guid>(value => value.Equals(expectedReward.RewardItemId)),
                It.Is<Guid>(value => value.Equals(score.DancerId))),
            Times.Once);
    }

    [Fact(DisplayName =
        "When dancer has reward, dancer qualifies for reward, qualified reward is lower quality, do not assign reward")]
    public async Task Execute_DancerHasNoReward_DancerQualifies_QualifiedRewardLowerQuality_DoNotAssignReward()
    {
        var reward = Guid.NewGuid();
        var score = new Score
        {
            Id = Guid.NewGuid(),
            DancerId = Guid.NewGuid(),
            ExScore = 15
        };
        var storedData = new SongScoreStrategyModel.StoredData()
        {
            Rewards = THRESHOLDS
        };
        var strategyModel = new SongScoreStrategyModel
        {
            Score = score,
            Data = storedData
        };
        var currentReward = new RewardQuality
        {
            Id = THRESHOLDS.First(r => r.MinExScore.Equals(50)).RewardItemId
        };

        _rewardQualityRepository.Setup(r =>
            r.GetRewardQualityForDancer(It.IsAny<Guid>(), It.IsAny<Guid>())
        ).Returns(currentReward);

        await _songScoreStrategy.Execute(reward, strategyModel, CancellationToken.None);

        _rewardQualityRepository.Verify(r =>
                r.GetRewardQualityForDancer(
                    It.Is<Guid>(value => value.Equals(reward)),
                    It.Is<Guid>(value => value.Equals(score.DancerId))),
            Times.Once());
        _rewardQualityRepository.Verify(r =>
                r.RemoveRewardFromDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _rewardQualityRepository.Verify(r =>
                r.AddRewardToDancer(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact(DisplayName =
        "When dancer has reward, dancer qualifies for reward, qualified reward is higer quality, remove old reward, assign new reward")]
    public async Task Execute_DancerHasNoReward_DancerQualifies_QualifiedRewardHigherQuality_RevokeOld_AssignNew()
    {
        var reward = Guid.NewGuid();
        var score = new Score
        {
            Id = Guid.NewGuid(),
            DancerId = Guid.NewGuid(),
            ExScore = 65
        };
        var storedData = new SongScoreStrategyModel.StoredData()
        {
            Rewards = THRESHOLDS
        };
        var strategyModel = new SongScoreStrategyModel
        {
            Score = score,
            Data = storedData
        };
        var currentReward = new RewardQuality
        {
            Id = THRESHOLDS.First(r => r.MinExScore.Equals(10)).RewardItemId
        };

        _rewardQualityRepository.Setup(r =>
            r.GetRewardQualityForDancer(It.IsAny<Guid>(), It.IsAny<Guid>())
        ).Returns(currentReward);

        var expectedReward = THRESHOLDS.First(r => r.MinExScore.Equals(50));

        await _songScoreStrategy.Execute(reward, strategyModel, CancellationToken.None);

        _rewardQualityRepository.Verify(r =>
                r.GetRewardQualityForDancer(
                    It.Is<Guid>(value => value.Equals(reward)),
                    It.Is<Guid>(value => value.Equals(score.DancerId))),
            Times.Once());
        _rewardQualityRepository.Verify(r =>
                r.RemoveRewardFromDancer(
                    It.Is<Guid>(value => value.Equals(currentReward.Id)),
                    It.Is<Guid>(value => value.Equals(score.DancerId)),
                    It.IsAny<CancellationToken>()),
            Times.Once);
        _rewardQualityRepository.Verify(r =>
                r.AddRewardToDancer(
                    It.Is<Guid>(value => value.Equals(expectedReward.RewardItemId)),
                    It.Is<Guid>(value => value.Equals(score.DancerId))),
            Times.Once);
    }

    [Fact(DisplayName =
        "When dancer has reward, dancer qualifies for reward, reward is same, skip assigning")]
    public async Task Execute_DancerHasNoReward_DancerQualifies_RewardIsSame_SkipAssign()
    {
        var reward = Guid.NewGuid();
        var score = new Score
        {
            Id = Guid.NewGuid(),
            DancerId = Guid.NewGuid(),
            ExScore = 65
        };
        var storedData = new SongScoreStrategyModel.StoredData()
        {
            Rewards = THRESHOLDS
        };
        var strategyModel = new SongScoreStrategyModel
        {
            Score = score,
            Data = storedData
        };
        var currentReward = new RewardQuality
        {
            Id = THRESHOLDS.First(r => r.MinExScore.Equals(50)).RewardItemId
        };

        _rewardQualityRepository.Setup(r =>
            r.GetRewardQualityForDancer(It.IsAny<Guid>(), It.IsAny<Guid>())
        ).Returns(currentReward);

        await _songScoreStrategy.Execute(reward, strategyModel, CancellationToken.None);

        _rewardQualityRepository.Verify(r =>
                r.GetRewardQualityForDancer(
                    It.Is<Guid>(value => value.Equals(reward)),
                    It.Is<Guid>(value => value.Equals(score.DancerId))),
            Times.Once());
        _rewardQualityRepository.Verify(r =>
                r.RemoveRewardFromDancer(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _rewardQualityRepository.Verify(r =>
                r.AddRewardToDancer(It.IsAny<Guid>(), It.IsAny<Guid>()),
            Times.Never);
    }
}