using System;
using System.Threading.Tasks;

namespace Application.Core.Interfaces;

public interface IRewardStrategy<in T>
{
    public Task Execute(Guid rewardId, T strategyData);
}