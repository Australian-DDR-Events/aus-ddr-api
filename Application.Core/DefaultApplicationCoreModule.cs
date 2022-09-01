using Application.Core.Interfaces;
using Application.Core.Interfaces.Repositories;
using Application.Core.Interfaces.Services;
using Application.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core
{
    public static class DefaultApplicationCoreModule
    {
        public static IServiceCollection LoadDefaultApplicationCoreModule(
            this IServiceCollection services)
        {
            return services
                .AddScoped<IDancerService, DancerService>()
                .AddScoped<IEventService, EventService>()
                .AddScoped<IBadgeService, BadgeService>()
                .AddScoped<IAdminService, AdminService>()
                .AddScoped<ISongService, SongService>()
                .AddScoped<ISongDifficultyService, SongDifficultyService>();
        }
    }
}