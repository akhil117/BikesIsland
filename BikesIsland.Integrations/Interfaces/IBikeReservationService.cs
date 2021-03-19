
using BikesIsland.Integrations.Common;
using BikesIsland.Models.Entities;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Interfaces
{
    public interface IBikeReservationService
    {
        Task<OperationResponse<BikeReservation>> MakeReservationAsync(BikeReservation bikeReservation, string name, string phoneNumber);
    }
}
