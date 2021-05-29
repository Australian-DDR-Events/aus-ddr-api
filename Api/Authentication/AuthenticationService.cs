using System;
using AusDdrApi.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AusDdrApi.Authentication
{
    public static class AuthenticationService
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["AuthScheme"] == "local")
            {
                services
                    .AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            }
            else
            {
                services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = configuration["Firebase:Url"];
                        options.Audience = configuration["Firebase:Audience"];
                    });
            }
            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("Admin", policy => policy.RequireClaim(UserContext.AdminClaimType));
                });
        }
    }
}