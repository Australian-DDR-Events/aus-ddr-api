using System;
using System.Collections.Generic;
using Application.Core;
using AusDdrApi.Attributes;
using AusDdrApi.Authentication;
using AusDdrApi.Extensions;
using AusDdrApi.Middleware;
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
                options => options.UseNpgsql(Configuration.GetConnectionString("DatabaseContext")).LogTo(Console.WriteLine));
            services.AddScoped(
                sp => sp.GetRequiredService<IDbContextFactory<EFDatabaseContext>>().CreateDbContext());
            
            services.AddControllers()
                .AddNewtonsoftJson(c => c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Australian DDR Events API", Version = "v1" });
                c.EnableAnnotations();

                var scheme = Configuration["AuthScheme"] == "local" ? "Basic" : "Bearer";
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = scheme
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = scheme
                            },
                            Scheme = "oauth2",
                            Name = scheme,
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
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
                                "https://stg.ausddrevents.com",
                                "https://ausddrevents.com")
                            .WithHeaders(HeaderNames.Authorization)
                            .AllowCredentials()
                            .AllowAnyMethod();
                    });
            });

            Console.WriteLine(JsonConvert.SerializeObject(Configuration));
            
            services.LoadDefaultApplicationCoreModule();
            services.LoadDefaultInfrastructureModule(Configuration);

            services.AddHttpContextAuthorizationServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Local"))
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

            using var serviceScope =
                app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<EFDatabaseContext>().Database.Migrate();
        }
    }
}