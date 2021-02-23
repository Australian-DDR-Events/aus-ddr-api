using System;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Authentication
{
    public static class UserContextExtension
    {
        public static string GetUserId(this HttpContext context)
        {
            var userId = context.Items[UserContext.UserIdClaimType]?.ToString();
            if (userId == null) throw new UnauthorizedAccessException();

            return userId;
        }

        public static void EnforceAdmin(this HttpContext context)
        {
            var admin = context.Items[UserContext.AdminClaimType]?.ToString();
            if (admin != "true") throw new UnauthorizedAccessException();
        }
    }
}