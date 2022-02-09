using System;
using System.Collections.Generic;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public record GetDancerBadgesByIdResponse(Guid Id, string Name, string Description, string EventName);