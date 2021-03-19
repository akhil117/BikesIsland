using Azure.Cosmos;
using BikesIsland.Configurations.Models;
using BikesIsland.Integrations.Interfaces;
using BikesIsland.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Services
{
    public abstract class CosmosDbDataRepository<T> : IDataRepository<T> where T : BaseEntity
    {

        protected readonly Configure _configure;
        private readonly ILogger<CosmosDbDataRepository<T>> _logger;
        private readonly CosmosClient _client;
        public CosmosDbDataRepository(IOptions<Configure> config, ILogger<CosmosDbDataRepository<T>> logger, CosmosClient client)
        {
            _configure = config.Value;
            _logger = logger;
            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public abstract string ContainerName { get; }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<T> itemResponse = await container.CreateItemAsync<T>(newEntity);
                return itemResponse.Value;
            }catch(Exception ex)
            {
                Log.Error($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");
                throw new Exception($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");
            }
        }

        public async Task DeleteAsync(string entityId,string partionKey)
        {
            try
            {
                CosmosContainer container = GetContainer();
                await container.DeleteItemAsync<T>(entityId, new PartitionKey(partionKey));
            }catch(Exception ex)
            {
                Log.Error($"Entity with ID: {entityId} was not removed successfully - error details: {ex.Message}");
            }

        }

        public async Task<T> GetAsync(string entityId,string partionKey)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(partionKey));
                return entityResult.Value;
            }
            catch (CosmosException ex)
            {
                Log.Error($"Entity with ID: {entityId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }
                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity, string partionKey)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<BaseEntity> entityResult = await container
                                                           .ReadItemAsync<BaseEntity>(entity.Id.ToString(), new PartitionKey(partionKey));

                if (entityResult != null)
                {
                    await container
                          .ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));
                }
                return entity;
            }
            catch (CosmosException ex)
            {
                Log.Error($"Entity with ID: {entity.Id} was not updated successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
            }
        }


        public CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_configure.CosmosDbSettings.DatabaseName);
            var container = database.GetContainer(ContainerName);
            return container;
        }
    }
}
