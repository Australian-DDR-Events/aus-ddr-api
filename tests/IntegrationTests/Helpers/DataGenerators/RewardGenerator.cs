using System;
using Application.Core.Entities;

namespace IntegrationTests.Helpers.DataGenerators;

public static class RewardGenerator
{
    public static Reward CreateReward() => new Reward()
    {
        Id = Guid.NewGuid()
    };
}