using System.Collections.Generic;

namespace Application.Core.Entities;

public class Reward : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string TriggerData { get; set; }
    public TriggerType Type { get; set; }
    public ICollection<RewardTrigger> Triggers { get; set; }

    public enum TriggerType
    {
        SongScore
    }
}