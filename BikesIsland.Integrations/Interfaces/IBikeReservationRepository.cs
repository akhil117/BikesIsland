using BikesIsland.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Interfaces
{
    public interface IBikeReservationRepository : IDataRepository<BikeReservation>
    {
        Task<BikeReservation> GetExistingReservationByBikeIdAsync(string carId, DateTime rentFrom);
    }
}
