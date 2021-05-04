using AusDdrApi.Services.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AusDdrApi.Extensions
{
    public static class HttpContextAuthorizationExtensions
    {
        public static IServiceCollection AddHttpContextAuthorizationServices(
            this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IAuthorization, HttpContextAuthorization>();
            
            return services;
        }
    }
}