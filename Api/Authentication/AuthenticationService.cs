using System;
using AusDdrApi.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
                Console.WriteLine($"Enabling bearer auth for issuer {configuration["oauth2identity:Issuer"]} and userinfo from {configuration["oauth2identity:UserInfoEndpoint"]}");
                services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = configuration["oauth2identity:Issuer"];
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                        };
                    });
            }
        }
    }
}