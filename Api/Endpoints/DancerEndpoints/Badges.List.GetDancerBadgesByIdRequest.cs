using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancerBadgesByIdRequest
{
    [FromRoute]
    public Guid Id { get; set; }
}