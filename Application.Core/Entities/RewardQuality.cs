using System;
using System.Collections.Generic;

namespace Application.Core.Entities;

public class RewardQuality : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid RewardId { get; set; }
    public Reward Reward { get; set; }

    public ICollection<Dancer> Dancers { get; set; } = new List<Dancer>();
}