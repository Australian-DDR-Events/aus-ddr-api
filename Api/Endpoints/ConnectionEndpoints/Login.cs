using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.ConnectionEndpoints;

[ApiController]
public class Login : ControllerBase
{
    private readonly IIdentity<string> _identity;

    public Login(IIdentity<string> identity)
    {
        _identity = identity;
    }

    [HttpPost("/connections/login")]
    public async Task<ActionResult<bool>> HandleAsync([FromQuery] string code, CancellationToken cancellationToken = new())
    {
        var result = await _identity
            .CreateSession(code);
        if (!result.Any())
        {
            return BadRequest();
        }
        Response.Cookies.Append("x-auth-cookie", result, new CookieOptions()
        {
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
        return Ok();
    }
}