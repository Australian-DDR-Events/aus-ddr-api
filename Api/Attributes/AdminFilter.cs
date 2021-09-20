using System;
using System.Linq;
using Application.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AusDdrApi.Attributes
{
    public class AdminFilter : IActionFilter
    {
        private readonly IIdentity<string> _identity;
        
        public AdminFilter(IIdentity<string> identity)
        {
            _identity = identity;
        }
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.Contains(" "))
            {
                context.Result = new ForbidResult();
                return;
            }
            var splitHeader = authHeader.Split(" ");
            if (!splitHeader[0].Equals("bearer", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
                return;
            }
            var token = authHeader.Split(" ")[1];
            if (!_identity.IsAdmin(token))
            {
                context.Result = new ForbidResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}