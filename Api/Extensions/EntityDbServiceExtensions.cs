using AusDdrApi.Services.Badges;
using AusDdrApi.Services.CoreData;
using AusDdrApi.Services.Dancer;
using AusDdrApi.Services.Dish;
using AusDdrApi.Services.Event;
using AusDdrApi.Services.GradedDancerDish;
using AusDdrApi.Services.GradedDancerIngredient;
using AusDdrApi.Services.GradedDish;
using AusDdrApi.Services.GradedIngredient;
using AusDdrApi.Services.Ingredient;
using AusDdrApi.Services.Score;
using AusDdrApi.Services.Song;
using Microsoft.Extensions.DependencyInjection;

namespace AusDdrApi.Extensions
{
    public static class EntityDbServiceExtensions
    {
        public static IServiceCollection AddDbEntityServices(
            this IServiceCollection services)
        {
            services
                .AddTransient<ICoreData, DbCoreData>()
                .AddTransient<ISong, DbSong>()
                .AddTransient<IScore, DbScore>()
                .AddTransient<IDancerService, DancerService>()
                .AddTransient<IGradedDancerDish, DbGradedDancerDish>()
                .AddTransient<IGradedDancerIngredient, DbGradedDancerIngredient>()
                .AddTransient<IGradedIngredient, DbGradedIngredient>()
                .AddTransient<IIngredient, DbIngredient>()
                .AddTransient<IGradedDish, DbGradedDish>()
                .AddTransient<IDish, DbDish>()
                .AddTransient<IEvent, DbEvent>()
                .AddTransient<IBadge, DbBadge>();
            
            return services;
        }
    }
}