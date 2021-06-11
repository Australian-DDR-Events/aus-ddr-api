using System;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Application.Core;
using AusDdrApi.Authentication;
using AusDdrApi.Context;
using AusDdrApi.Middleware;
using AusDdrApi.Services.FileStorage;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AusDdrApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<EFDatabaseContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext")));
            services.AddScoped(
                sp => sp.GetService<IDbContextFactory<EFDatabaseContext>>().CreateDbContext());
            services.AddControllers()
                .AddNewtonsoftJson(c => c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Australian DDR Events API", Version = "v1" });
                c.EnableAnnotations();
            });
            
            services.AddJwtAuthentication(Configuration);
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(
                                "http://localhost:1234",
                                "https://ausddrevents-e18b1--ausddrevents-staging-homst7uj.web.app",
                                "https://ausddrevents-e18b1--ausddrevents-development-di3otvtf.web.app",
                                "https://ausddrevents.com")
                            .WithHeaders(HeaderNames.Authorization);
                    });
            });

            switch (Configuration["FileStorage"])
            {
                case "filesystem":
                {
                    var basePath = Configuration["FilesystemConfig:BasePath"];
                    if (string.IsNullOrEmpty(basePath))
                    {
                        basePath = AppDomain.CurrentDomain.BaseDirectory + "/filestorage";
                    }

                    services.AddSingleton<IFileStorage>(new LocalFileStorage(basePath));
                    break;
                }
                default:
                {
                    var credentials = new BasicAWSCredentials(Configuration["AwsAccessKey"], Configuration["AwsSecretKey"]);
                    var region = RegionEndpoint.GetBySystemName(Configuration["AwsConfiguration:Region"]);
                    var client = new AmazonS3Client(credentials, region);
                    var awsConfiguration = Configuration.GetSection("AwsConfiguration").Get<AwsConfiguration>();
            
                    services.AddSingleton<IFileStorage>(new S3FileStorage(client, awsConfiguration));
                    break;
                }
                        
            }

            services.LoadDefaultApplicationCoreModule();
            services.LoadDefaultInfrastructureModule();

            //services.AddHttpContextAuthorizationServices();
            //services.AddGraphQLConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOptions();

            app.UseAuthentication();
            
            app.UseRouting();

            app.UseCors("CorsPolicy");
            
            app.UseAuthorization();
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.Use(UserContext.UseUserContext);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}