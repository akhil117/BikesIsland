using Azure;
using Azure.Cosmos;
using BikesIsland.Configurations.Models;
using BikesIsland.Integrations.Interfaces;
using BikesIsland.Models.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Services
{
    public class BikeReservationRepository : CosmosDbDataRepository<BikeReservation>, IBikeReservationRepository
    {
        public BikeReservationRepository(IOptions<Configure> config, ILogger<CosmosDbDataRepository<BikeReservation>> logger, CosmosClient client) : base(config, logger, client)
        {
        }

        public override string ContainerName => _configure.CosmosDbSettings.BikeContainerName;

        public async Task<BikeReservation> GetExistingReservationByBikeIdAsync(string carId, DateTime rentFrom)
        {
            try
            {
                CosmosContainer container = GetContainer();
                var entities = new List<BikeReservation>();
                QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.rentTo > @rentFrom AND c.carId = @carId")
                    .WithParameter("@rentFrom", rentFrom)
                    .WithParameter("@carId", carId);

                AsyncPageable<BikeReservation> queryResultSetIterator = container.GetItemQueryIterator<BikeReservation>(queryDefinition);

                await foreach (BikeReservation BikeReservation in queryResultSetIterator)
                {
                    entities.Add(BikeReservation);
                }

                return entities.FirstOrDefault();
            }
            catch (CosmosException ex)
            {
                Log.Error($"Entity with ID: {carId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
            }
        }
    }
}
