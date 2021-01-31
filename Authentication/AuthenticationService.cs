using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AusDdrApi.Authentication
{
    public static class AuthenticationService
    {
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://securetoken.google.com/ausddrevents-e18b1";
                    options.Audience = "ausddrevents-e18b1";
                });
        
        }
    }
}