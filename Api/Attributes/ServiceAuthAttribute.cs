using System;
using System.Linq;
using Application.Core.Interfaces;
using Application.Core.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace AusDdrApi.Attributes;

public class ServiceAuthAttribute : ActionFilterAttribute
{
    public string? Scope { get; set; }
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var service = context.HttpContext.RequestServices;
        var identity = service.GetService<IIdentity<string>>();
        if (identity == null)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return;
        }
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
        if (identity.GetTokenType(token) != TokenType.SERVICE)
        {
            context.Result = new ForbidResult();
        }
        if (Scope != null && !identity.HasScope(token, Scope))
        {
            context.Result = new ForbidResult();
        }
    }
}