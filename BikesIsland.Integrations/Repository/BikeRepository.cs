using Azure.Cosmos;
using BikesIsland.Configurations.Models;
using BikesIsland.Integrations.Services;
using BikesIsland.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikesIsland.Integrations.Repository
{
    public class BikeRepository : CosmosDbDataRepository<Bike>
    {
        public BikeRepository(IOptions<Configure> config, ILogger<CosmosDbDataRepository<Bike>> logger, CosmosClient client) : base(config, logger, client)
        {
        }

        public override string ContainerName => _configure.CosmosDbSettings.BikeContainerName;

    }
}
