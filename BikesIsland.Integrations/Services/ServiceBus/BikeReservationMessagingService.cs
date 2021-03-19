using Azure.Messaging.ServiceBus;
using BikesIsland.Integrations.Interfaces.ServiceBus;
using BikesIsland.Models.Entities;
using BikesIsland.Models.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Services.ServiceBus
{
    public class BikeReservationMessagingService : IBikeReservationMessagingService
    {
        private readonly ServiceBusSender _serviceBusSender;
        public BikeReservationMessagingService(ServiceBusSender serviceBusSender)
        {
            _serviceBusSender = serviceBusSender;
        }
        public async Task PublishNewBikeReservationMessageAsync(BikeReservation bikeReservation)
        {
            var bike = new BikeReservationIntegrationMessage
            {
                Id = Guid.NewGuid().ToString(),
                BikeId = bikeReservation.BikeId,
                CustomerId = bikeReservation.CustomerId,
                RentFrom = bikeReservation.RentFrom,
                RentTo = bikeReservation.RentTo
            };
            var serializedMessage = JsonSerializer.Serialize(bike);
            ServiceBusMessage message = new ServiceBusMessage(serializedMessage);
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
