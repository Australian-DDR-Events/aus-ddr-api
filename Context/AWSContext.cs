using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;

namespace AusDdrApi.Context
{
    public class AWSConfiguration
    {
        public string AssetsBucketName { get; set; }
        public string AssetsBucketLocation { get; set; }
    }
    public static class AWSContext
    {
        public static IApplicationBuilder UseAWSContext(this IApplicationBuilder app, IConfiguration configuration)
        {
            var awsConfiguration = configuration.GetSection("AWSConfiguration").Get<AWSConfiguration>();

            return app.Use((context, func) =>
            {
                context.Items["AWSConfiguration"] = awsConfiguration;
                return func();
            });
        }
    }
}