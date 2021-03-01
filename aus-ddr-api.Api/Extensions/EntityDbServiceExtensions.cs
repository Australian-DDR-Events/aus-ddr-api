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
            services.AddTransient<ICoreData, DbCoreData>();
            services.AddTransient<ISong, DbSong>();
            services.AddTransient<IScore, DbScore>();
            services.AddTransient<IDancer, DbDancer>();
            services.AddTransient<IGradedDancerDish, DbGradedDancerDish>();
            services.AddTransient<IGradedDancerIngredient, DbGradedDancerIngredient>();
            services.AddTransient<IGradedIngredient, DbGradedIngredient>();
            services.AddTransient<IIngredient, DbIngredient>();
            services.AddTransient<IGradedDish, DbGradedDish>();
            services.AddTransient<IDish, DbDish>();
            services.AddTransient<IEvent, DbEvent>();
            services.AddTransient<IBadge, DbBadge>();
            
            return services;
        }
    }
}