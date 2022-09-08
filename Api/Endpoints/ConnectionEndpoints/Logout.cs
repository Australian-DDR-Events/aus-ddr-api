using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AusDdrApi.Endpoints.ConnectionEndpoints;

[ApiController]
public class Logout : ControllerBase
{
    private readonly IIdentity<string> _identity;

    public Logout(IIdentity<string> identity)
    {
        _identity = identity;
    }

    [HttpDelete("/connections/logout")]
    public async Task<ActionResult<bool>> HandleAsync(CancellationToken cancellationToken = new())
    {
        if (HttpContext.Items["cookie"] is string cookie && cookie.Any())
        {
            await _identity.ClearSession(cookie);
        }
        Response.Cookies.Append("x-auth-cookie", "", new CookieOptions()
        {
            Secure = true,
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow
        });
        return Ok();
    }
}