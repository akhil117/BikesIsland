using BikesIsland.Integrations.Interfaces;
using BikesIsland.Integrations.Services;
using BikesIsland.Configurations.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.IO;
using BikesIsland.Models.Entities;
using BikesIsland.Integrations.Repository;
using Microsoft.OpenApi.Models;
using Azure.Storage.Blobs;
using BikesIsland.Integrations.Services.Storage;
using BikesIsland.Integrations.Interfaces.Storage;
using Azure.Messaging.ServiceBus;
using BikesIsland.Integrations.Services.ServiceBus;
using BikesIsland.Integrations.Interfaces.ServiceBus;
using Azure.Cosmos;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace BikesIsland.API
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        //IHostEnvironment
        public Startup(IHostEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen();
            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ICosmosDbService,CosmosDbService>();
            services.Configure<Configure>(Configuration);
            services.AddSingleton<IDataRepository<Bike>, BikeRepository>();
            var value = Configuration.GetSection("BlobStorageSettings:ConnectionString").Value;
            //need to add connectionstring
            services.AddSingleton(factory => new BlobServiceClient(value));
            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            services.AddSingleton<IBikeReservationRepository,BikeReservationRepository>();
            services.AddSingleton<IBikeReservationService, BikeReservationService>();

            //adding asbconfiguration
            AsbConfiguration(services);
            SetUpCosmosDB(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Bikes Island in Azure",
                    Version = "v1",
                    Description = "Integrating all the AZ services for Bikes Island"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }


        private void SetUpCosmosDB(IServiceCollection services)
        {

            services.AddSingleton(implementationFactory =>
            {
                CosmosClient cosmosClient = new CosmosClient(Configuration.GetSection("CosmosDbSettings:ConnectionString").Value);

                return cosmosClient;

            });

        }

        private void AsbConfiguration(IServiceCollection services)
        {
            services.AddSingleton(factory =>
            {
                var serviceBusClient = new ServiceBusClient(Configuration.GetSection("ServiceBusSettings:ConnectionString").Value);
                var serviceBusSender = serviceBusClient.CreateSender(Configuration.GetSection("ServiceBusSettings:QueueName").Value);
                return serviceBusSender;
            });
            services.AddSingleton<IBikeReservationMessagingService, BikeReservationMessagingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

                app.UseDeveloperExceptionPage();
           

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.DocumentTitle = "Bikes Island in Azure";
            });

            app.UseHealthChecks("/", new HealthCheckOptions()
            {
                ResponseWriter = WriteResponse
            });
        }

        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("Container", System.Net.Dns.GetHostName()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
