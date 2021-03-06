using System;
using AusDdrApi.Authentication;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Services.Authorization
{
    public class HttpContextAuthorization : IAuthorization
    {
        private readonly IHttpContextAccessor _context;

        public HttpContextAuthorization(IHttpContextAccessor context)
        {
            _context = context;
        }
        
        public string? GetUserId()
        {
            var userId = _context.HttpContext?.Items[UserContext.UserIdClaimType]?.ToString();

            return userId;
        }

        public void EnforceAdmin()
        {
            var admin = _context.HttpContext?.Items[UserContext.AdminClaimType]?.ToString();
            if (admin != "true") throw new UnauthorizedAccessException();
        }
    }
}