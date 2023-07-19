using System;
using System.Collections.Generic;

namespace Application.Core.Models.Strategies;

public class SongScoreStrategyModel
{
    public Entities.Score Score { get; set; }
    public StoredData Data { get; set; }

    public class StoredData
    {
        public List<ScoreRewardThreshold> Rewards { get; set; }
    }

    public class ScoreRewardThreshold
    {
        public Guid RewardItemId { get; set; }
        public int? MinExScore { get; set; }
    }
}