using System;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class GetDancerBadgesByIdRequest
{
    public const string Route = "/dancers/{Id:guid}/badges";
    
    public static string BuildRoute(Guid id) => Route.Replace("{Id:guid}", id.ToString());
        
    [FromRoute]
    public Guid Id { get; set; }
}