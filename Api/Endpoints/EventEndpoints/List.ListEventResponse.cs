using System;

namespace AusDdrApi.Endpoints.EventEndpoints;

public record ListEventResponse(Guid EventId, string Name, string Description, DateTime startDate, DateTime endDate);