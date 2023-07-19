using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.BadgeEndpoints;

public class GetBadgesRequest
{
    public const string Route = "/badges";
    
    [FromQuery]
    public int? Page { get; set; } = 0;
    
    [FromQuery]
    [Range(1, 100, ErrorMessage = "Cannot request more than 100 badges per request")]
    public int? Limit { get; set; }
}
