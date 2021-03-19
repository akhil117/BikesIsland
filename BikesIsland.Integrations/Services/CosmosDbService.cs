using BikesIsland.Configurations.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Azure.Cosmos;
using BikesIsland.Integrations.Interfaces;

namespace BikesIsland.Integrations.Services
{
   public class CosmosDbService : ICosmosDbService
    {
        public Configure _config;
        public ILogger<CosmosDbService> _logger;
        public CosmosDatabase _database;
        public CosmosDbService(IOptions<Configure> config,ILogger<CosmosDbService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public void SetUpCosmosDbService()
        {
            SetUpCosmosDB();
        }

        private void SetUpCosmosDB()
        {
            var data = _config.CosmosDbSettings.ConnectionString;
            CosmosClient cosmosClient = new CosmosClient(_config.CosmosDbSettings.ConnectionString);
            _database = cosmosClient.CreateDatabaseIfNotExistsAsync(_config.CosmosDbSettings.DatabaseName)
                                                   .GetAwaiter()
                                                   .GetResult();

            _database.CreateContainerIfNotExistsAsync(
                _config.CosmosDbSettings.BikeContainerName,
                _config.CosmosDbSettings.BikePartitionKeyPath,
                400)
                .GetAwaiter()
                .GetResult();

            _database.CreateContainerIfNotExistsAsync(
                "Enquiry",
                "/customerAttachedEmail",
                400
                )
                .GetAwaiter()
                .GetResult();

            _database.CreateContainerIfNotExistsAsync(
                    _config.CosmosDbSettings.BikeReservationContainerName,
                     _config.CosmosDbSettings.BikeReservationPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();
        }


    }
}
