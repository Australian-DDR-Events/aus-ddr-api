using System;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancerByTokenResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
}
