using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Application.Core.Interfaces;
using AusDdrApi.Context;
using AusDdrApi.Services.FileStorage;
using Infrastructure.Data;
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
                .AddScoped(typeof(IAsyncRepository<>), typeof(GenericEfRepository<>))
                .AddFileStorage(configuration);
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
                    var credentials = new BasicAWSCredentials(configuration["AwsAccessKey"], configuration["AwsSecretKey"]);
                    var region = RegionEndpoint.GetBySystemName(configuration["AwsConfiguration:Region"]);
                    var client = new AmazonS3Client(credentials, region);
                    var awsConfiguration = configuration.GetSection("AwsConfiguration").Get<AwsConfiguration>();
            
                    services.AddSingleton<IFileStorage>(new S3FileStorage(client, awsConfiguration));
                    break;
                }         
            }

            return services;
        }
    }
}