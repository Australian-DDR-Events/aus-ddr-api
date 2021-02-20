using System;
using AusDdrApi.Persistence;
using AusDdrApi.Services.Entities.CoreService;
using AusDdrApi.Services.Entities.DancerService;
using AusDdrApi.Services.Entities.ScoreService;
using AusDdrApi.Services.Entities.SongService;
using Microsoft.Extensions.DependencyInjection;

namespace AusDdrApi.Extensions
{
    public static class EntityDbServiceExtensions
    {
        public static IServiceCollection AddDbEntityServices(
            this IServiceCollection services)
        {
            services.AddTransient<ICoreService, DbCoreService>();
            services.AddTransient<ISongService, DbSongService>();
            services.AddTransient<IScoreService, DbScoreService>();
            services.AddTransient<IDancerService, DbDancerService>();
            return services;
        }
    }
}