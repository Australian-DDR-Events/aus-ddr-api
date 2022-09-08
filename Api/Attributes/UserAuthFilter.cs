using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AusDdrApi.Attributes;

public class UserAuthFilter : IAsyncActionFilter
{
    private readonly IIdentity<string> _identity;

    private const string USER_COOKIE = "x-auth-cookie";
    
    public UserAuthFilter(IIdentity<string> identity)
    {
        _identity = identity;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        context.HttpContext.Request.Cookies.TryGetValue(USER_COOKIE, out var cookie);
        if (cookie == null || !cookie.Any())
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        if (!_identity.IsSessionActive(cookie))
        {
            cookie = await _identity.RefreshSession(cookie);
            if (!cookie.Any())
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.Response.Cookies.Append(USER_COOKIE, cookie, new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
        }
        context.HttpContext.Items.Add("cookie", cookie);
        await next();
    }
}