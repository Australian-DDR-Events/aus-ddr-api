using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using AusDdrApi.Authentication;
using AusDdrApi.Context;
using AusDdrApi.Extensions;
using AusDdrApi.GraphQL;
using AusDdrApi.GraphQL.Dancers;
using AusDdrApi.GraphQL.DataLoader;
using AusDdrApi.GraphQL.DataLoader.Summer2021;
using AusDdrApi.GraphQL.Summer2021;
using AusDdrApi.GraphQL.Types;
using AusDdrApi.GraphQL.Types.Summer2021;
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
            services.AddPooledDbContextFactory<DatabaseContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext")));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
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
                .AddTypeExtension<Summer2021Queries>()
                
                // Mutations
                .AddMutationType(x => x.Name("Mutation"))
                .AddTypeExtension<DancerMutations>()

                // Types
                .AddType(new UuidType('D'))
                .AddType<DancerType>()
                .AddType<BadgeType>()
                .AddType<GradedIngredientType>()
                .AddType<GradedDancerIngredientType>()

                // Extensions
                .AddProjections()
                .AddFiltering()
                .AddSorting()
                .EnableRelaySupport()
                .AddAuthorization()

                // Data loaders
                .AddDataLoader<DancerByIdDataLoader>()
                .AddDataLoader<BadgeByIdDataLoader>()
                .AddDataLoader<ScoreByIdDataLoader>()
                .AddDataLoader<IngredientByIdDataLoader>()
                .AddDataLoader<GradedIngredientByIdDataLoader>()
                .AddDataLoader<IngredientByDancerIdDataLoader>();
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

            app.Use(UserContext.UseUserContext);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
            context?.Database.Migrate();
        }
    }
}