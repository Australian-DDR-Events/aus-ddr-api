using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Models.Strategies;

namespace Application.Core.Services.RewardStrategies;

public class SongScoreStrategy : IRewardStrategy<SongScoreStrategyModel>
{
    private readonly IRewardQualityRepository _rewardQualityRepository;
    
    public SongScoreStrategy(IRewardQualityRepository rewardQualityRepository)
    {
        _rewardQualityRepository = rewardQualityRepository;
    }
    
    public async Task Execute(Guid rewardId, SongScoreStrategyModel strategyData)
    {
        var currentUserReward =
            _rewardQualityRepository.GetRewardQualityForDancer(rewardId, strategyData.Score.DancerId);
        if (currentUserReward != null && SkipAssigningReward(currentUserReward.Id, strategyData.Score.ExScore,
                strategyData.Data.Rewards)) return;
        var rewardToAssign = strategyData
            .Data
            .Rewards
            .Where(r =>
                r.MinExScore == null || r.MinExScore <= strategyData.Score.ExScore
            ).MaxBy(r => r.MinExScore);
        if (currentUserReward != null)
            await _rewardQualityRepository.RemoveRewardFromDancer(currentUserReward.Id, strategyData.Score.DancerId);
        if (rewardToAssign != null)
            await _rewardQualityRepository.AddRewardToDancer(rewardToAssign.RewardItemId, strategyData.Score.DancerId);
    }

    private bool SkipAssigningReward(Guid currentRewardId, int exScore,
        IList<SongScoreStrategyModel.ScoreRewardThreshold> thresholds)
    {
        var currentThreshold = thresholds.First(t => t.RewardItemId.Equals(currentRewardId));
        var bestQualifiedReward = thresholds
            .Where(r =>
                r.MinExScore == null || r.MinExScore <= exScore)
            .MaxBy(r => r.MinExScore);
        return bestQualifiedReward == null || currentThreshold.MinExScore >= bestQualifiedReward.MinExScore;
    }
}