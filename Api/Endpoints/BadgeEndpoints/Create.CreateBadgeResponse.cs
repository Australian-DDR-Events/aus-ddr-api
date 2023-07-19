using System;

namespace AusDdrApi.Endpoints.BadgeEndpoints;

public class CreateBadgeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
};
