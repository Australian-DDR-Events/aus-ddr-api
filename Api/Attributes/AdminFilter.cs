using System;
using System.Linq;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AusDdrApi.Attributes
{
    public class AdminFilter : IActionFilter
    {
        private const string USER_COOKIE = "x-auth-cookie";

        private readonly IIdentity<string> _identity;
        
        public AdminFilter(IIdentity<string> identity)
        {
            _identity = identity;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue(USER_COOKIE, out var cookie);
            if (cookie == null || !cookie.Any())
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (!_identity.IsAdmin(cookie))
            {
                context.Result = new ForbidResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}