using BikesIsland.Integrations.Common;
using BikesIsland.Integrations.Interfaces;
using BikesIsland.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BikesIsland.Integrations.Services
{
    public class BikeReservationService : IBikeReservationService
    {
        private readonly IBikeReservationRepository _bikeReservationRepository;
        private readonly IDataRepository<Bike> _dataRepository;
        

        public BikeReservationService(IBikeReservationRepository bikeReservationRepository, IDataRepository<Bike> dataRepository)
        {
            _bikeReservationRepository = bikeReservationRepository ?? throw new ArgumentNullException(nameof(bikeReservationRepository)); ;
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
        }

        public async Task<OperationResponse<BikeReservation>> MakeReservationAsync(BikeReservation bikeReservation, string name,string phoneNumber)
        {
            var carFromReservation = await _dataRepository.GetAsync(bikeReservation.BikeId, bikeReservation.BikeId);
            if (carFromReservation == null)
            {
                return new OperationResponse<BikeReservation>()
                                       .SetAsFailureResponse(OperationErrorDictionary.BikeReservation.BikeDoesNotExist());
            }

            var existingCarReservation = await _bikeReservationRepository.GetExistingReservationByBikeIdAsync(bikeReservation.BikeId, bikeReservation.RentFrom);

            if (existingCarReservation != null)
            {
                return new OperationResponse<BikeReservation>()
                                       .SetAsFailureResponse(OperationErrorDictionary.BikeReservation.BikeAlreadyReserved());
            }

            else
            {
                bikeReservation.Id = Guid.NewGuid().ToString();
                bikeReservation.CustomerId = phoneNumber;
                bikeReservation.CustomerName = name;
                var createdCarReservation = await _bikeReservationRepository.AddAsync(bikeReservation);
                return new OperationResponse<BikeReservation>(createdCarReservation);
            }
        }

    }
}
