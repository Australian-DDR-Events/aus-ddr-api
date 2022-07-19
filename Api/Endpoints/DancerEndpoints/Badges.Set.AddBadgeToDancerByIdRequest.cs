using System;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class AddBadgeToDancerByIdRequest
{  
    [FromRoute]
    public Guid DancerId { get; set; }
    
    [FromRoute]
    public Guid BadgeId { get; set; }
}