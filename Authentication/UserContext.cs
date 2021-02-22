using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AusDdrApi.Authentication
{
    public static class UserContext
    {
        public static string UserIdClaimType = "user_id";
        public static string AdminClaimType = "admin";
        public static Task UseUserContext(HttpContext context, Func<Task> next)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null)
            {
                var token = authHeader.Split(" ")[1];
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                context.Items[UserIdClaimType] = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == UserIdClaimType)?.Value;
                context.Items[AdminClaimType] = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == AdminClaimType)?.Value;
            }
            return next();
        }
    }
}