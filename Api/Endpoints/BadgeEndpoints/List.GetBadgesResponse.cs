using System;

namespace AusDdrApi.Endpoints.BadgeEndpoints;
public record GetBadgesResponse(Guid id, string name);