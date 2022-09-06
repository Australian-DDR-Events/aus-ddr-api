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
        if (cookie == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        if (!_identity.IsSessionActive(cookie))
        {
            var newSession = await _identity.RefreshSession(cookie);
            if (newSession == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.Response.Cookies.Append(USER_COOKIE, newSession, new CookieOptions()
            {
                Secure = true,
                SameSite = SameSiteMode.Strict,
                HttpOnly = true
            });
        }

        await next();
    }
    

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new System.NotImplementedException();
    }
}