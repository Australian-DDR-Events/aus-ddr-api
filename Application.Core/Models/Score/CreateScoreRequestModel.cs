using System;

namespace Application.Core.Models.Score;

public class CreateScoreRequestModel
{
    public Guid ChartId { get; set; }
    public Guid DancerId { get; set; }
    public int Score { get; set; }
    public int ExScore { get; set; }
}