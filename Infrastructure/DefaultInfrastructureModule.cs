using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Application.Core.Interfaces;
using Application.Core.Interfaces.ExternalServices;
using Application.Core.Interfaces.Repositories;
using AusDdrApi.Context;
using AusDdrApi.Services.FileStorage;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.Data.Internal;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DefaultInfrastructureModule
    {
        public static IServiceCollection LoadDefaultInfrastructureModule(
            this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<ICache, InMemoryCache>()
                .AddIdentity(configuration)
                .AddScoped(typeof(IAsyncRepository<>), typeof(GenericEfRepository<>))
                .AddScoped<ISongRepository, SongRepository>()
                .AddScoped<IEventRepository, EventRepository>()
                .AddScoped<IDancerRepository, DancerRepository>()
                .AddScoped<IBadgeRepository, BadgeRepository>()
                .AddScoped<IRewardRepository, RewardRepository>()
                .AddScoped<IChartRepository, ChartRepository>()
                .AddScoped<IConnectionRepository, ConnectionRepository>()
                .AddScoped<ISessionRepository, SessionRepository>()
                .AddHttpClient()
                .AddFileStorage(configuration)
                .AddDiscordApi(configuration)
                .AddSingleton<ILogger, ConsoleLogger>();
        }

        private static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration["FileStorage"])
            {
                case "filesystem":
                {
                    var basePath = configuration["FilesystemConfig:BasePath"];
                    if (string.IsNullOrEmpty(basePath))
                    {
                        basePath = AppDomain.CurrentDomain.BaseDirectory + "/filestorage";
                    }

                    services.AddSingleton<IFileStorage>(new LocalFileStorage(basePath));
                    break;
                }
                default:
                {
                    var credentials = new InstanceProfileAWSCredentials();
                    var region = RegionEndpoint.GetBySystemName(configuration["AwsConfiguration:Region"]);
                    var client = new AmazonS3Client(credentials, region);
                    var awsConfiguration = configuration.GetSection("AwsConfiguration").Get<AwsConfiguration>();
            
                    services.AddSingleton<IFileStorage>(new S3FileStorage(client, awsConfiguration));
                    break;
                }         
            }

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration["AuthScheme"].Equals("local", StringComparison.InvariantCulture))
            {
                return services.AddSingleton(typeof(IIdentity<string>), typeof(LocalIdentity));
            }
            var oauth2IdentityConfig = new OAuth2IdentityConfig();
            configuration.GetSection("oauth2identity")
                .Bind(oauth2IdentityConfig, c => c.BindNonPublicProperties = true);
            services.AddHttpClient<IIdentity<string>, OAuth2Identity>();
            return services
                .AddSingleton(oauth2IdentityConfig)
                .AddSingleton(typeof(IIdentity<string>), typeof(OAuth2Identity));
        }

        private static IServiceCollection AddDiscordApi(this IServiceCollection services, IConfiguration configuration)
        {
            var discordApiConfig = new DiscordApiConfiguration();
            configuration.GetSection("discordapi")
                .Bind(discordApiConfig, c => c.BindNonPublicProperties = true);
            return services
                .AddSingleton(discordApiConfig)
                .AddScoped<IDiscordApiService, DiscordApiService>();
        }
    }
}