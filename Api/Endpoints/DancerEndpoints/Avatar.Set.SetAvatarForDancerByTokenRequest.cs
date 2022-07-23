using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Endpoints.DancerEndpoints;

public class SetAvatarForDancerByTokenRequest
{
    [Required]
    public IFormFile? Image { get; set; }
}