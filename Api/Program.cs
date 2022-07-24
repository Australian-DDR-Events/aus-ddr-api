using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AusDdrApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(options =>
                {
                    options.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((hostBuilder, config) =>
                {
                    Console.WriteLine($"Loading environment for {hostBuilder.HostingEnvironment.EnvironmentName}");
                    var env = hostBuilder.HostingEnvironment;
                    if (!env.EnvironmentName.Equals("Local"))
                    {
                        config.AddSystemsManager($"/{hostBuilder.HostingEnvironment.EnvironmentName}/api-config/");
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}