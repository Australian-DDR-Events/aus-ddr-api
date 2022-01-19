using System;

namespace Application.Core.Models.Dancer;

public class MigrateDancerRequestModel
{
    public string AuthId { get; init; } = string.Empty;
    public string? LegacyId { get; init; }
}
