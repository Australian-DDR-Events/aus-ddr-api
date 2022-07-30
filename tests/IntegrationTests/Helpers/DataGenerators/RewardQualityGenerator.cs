using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class RewardQualityGenerator
{
    public static RewardQuality CreateRewardQuality(Reward reward) => new()
    {
        Id = Guid.NewGuid(),
        RewardId = reward.Id
    };
}