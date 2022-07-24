using System;
using System.Collections.Generic;

namespace Application.Core.Entities;

public class Reward : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TriggerData { get; set; } = string.Empty;
    public TriggerType Type { get; set; }
    public ICollection<RewardTrigger> Triggers { get; set; } = new List<RewardTrigger>();

    public enum TriggerType
    {
        SongScore
    }
}