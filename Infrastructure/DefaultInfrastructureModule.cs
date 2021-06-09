using Application.Core.Entities;
using Application.Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DefaultInfrastructureModule
    {
        public static IServiceCollection LoadDefaultInfrastructureModule(
            this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IAsyncRepository<>), typeof(GenericEfRepository<>));
        }
    }
}