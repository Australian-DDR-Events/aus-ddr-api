using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class SetAvatarForDancerByTokenRequest
{
    public const string Route = "/dancers/avatar";
    
    [Required]
    public IFormFile? Image { get; set; }
}