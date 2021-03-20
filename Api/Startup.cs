using System.Collections.Generic;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using AusDdrApi.Authentication;
using AusDdrApi.Context;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL;
using AusDdrApi.GraphQL.Dancers;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.Helpers;
using AusDdrApi.Middleware;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

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
            // services.AddDbContext<DatabaseContext>(options =>
            //     options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext")));
            
            services.AddPooledDbContextFactory<DatabaseContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext")));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddJwtAuthentication(Configuration);
            services.AddRouting(options => options.LowercaseUrls = true);
            /* services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AusDdrApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            }); */

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

            var credentials = new BasicAWSCredentials(Configuration["AwsAccessKey"], Configuration["AwsSecretKey"]);
            var region = RegionEndpoint.GetBySystemName(Configuration["AwsConfiguration:Region"]);
            var client = new AmazonS3Client(credentials, region);
            var awsConfiguration = Configuration.GetSection("AwsConfiguration").Get<AwsConfiguration>();
            
            services.AddSingleton<IFileStorage>(new S3FileStorage(client, awsConfiguration));

            services.AddHttpContextAuthorizationServices();
            // services.AddDbEntityServices();

            services
                .AddGraphQLServer()

                // Queries
                .AddQueryType(x => x.Name("Query"))
                .AddTypeExtension<DancerQueries>()
                
                // Mutations
                .AddMutationType(x => x.Name("Mutation"))
                .AddTypeExtension<DancerMutations>()

                // Types
                .AddType(new UuidType('D'))
                .AddType<DancerType>()

                // Extensions
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .EnableRelaySupport()

                // Data loaders
                .AddDataLoader<DancerByIdDataLoader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOptions();
            
            // app.UseSwagger();
            // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AusDdrApi v1"));

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.Use(UserContext.UseUserContext);

            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapControllers();
                endpoints.MapGraphQL();
            });

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
            context?.Database.Migrate();
        }
    }
}