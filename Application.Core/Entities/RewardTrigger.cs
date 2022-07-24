using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Core.Entities;

public class RewardTrigger
{
    [Key]
    public string Trigger { get; set; }
    
    public ICollection<Reward> Rewards { get; set; }
}