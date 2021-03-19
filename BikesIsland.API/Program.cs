using BikesIsland.Integrations.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Enrichers.AspnetcoreHttpcontext;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using BikesIsland.Logging.Configuration;

namespace BikesIsland.API
{
#pragma warning disable CS1591
    public class Program
    {

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
                                     .SetBasePath(Directory.GetCurrentDirectory())
                                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                     .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                                     .AddEnvironmentVariables()
                                     .Build();
        private static IWebHost host;
        public static void Main(string[] args)
        {
           host = BuildWebHost(args);
           var cosmosDbService = host.Services.GetService<ICosmosDbService>();
           cosmosDbService.SetUpCosmosDbService();
           host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((provider, context, loggerConfig) =>
                {
                    loggerConfig.WithBikesIslandConfiguration(provider, Configuration);
                })
                .Build();
    }
#pragma warning restore CS1591
}
