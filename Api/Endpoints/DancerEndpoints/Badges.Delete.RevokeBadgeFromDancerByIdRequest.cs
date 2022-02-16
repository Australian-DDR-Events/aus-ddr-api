using System;
using AusDdrApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class RevokeBadgeFromDancerByIdRequest
{
    public const string Route = "/dancers/{DancerId:guid}/badges/{BadgeId:guid}";
    
    public static string BuildRoute(Guid dancerId, Guid badgeId) => 
        Route.Replace("{DancerId:guid}", dancerId.ToString()).Replace("{BadgeId:guid", badgeId.ToString());
        
    [FromRoute]
    public Guid DancerId { get; set; }
    
    [FromRoute]
    public Guid BadgeId { get; set; }
}