using BikesIsland.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Interfaces.ServiceBus
{
    public interface IBikeReservationMessagingService
    {
        Task PublishNewBikeReservationMessageAsync(BikeReservation carReservation);
    }
}
