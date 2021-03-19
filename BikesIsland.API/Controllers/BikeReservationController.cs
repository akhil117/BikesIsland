using AutoMapper;
using BikesIsland.Integrations.Interfaces;
using BikesIsland.Integrations.Interfaces.ServiceBus;
using BikesIsland.Models.Dto;
using BikesIsland.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BikesIsland.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BikeReservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBikeReservationService _bikeReservationService;
        private readonly IBikeReservationMessagingService _bikeReservationMessagingService;
        public BikeReservationController(IMapper mapper,IBikeReservationService bikeReservationService, IBikeReservationMessagingService bikeReservationMessagingService)
        {
            _bikeReservationService = bikeReservationService;
            _bikeReservationMessagingService = bikeReservationMessagingService;
            _mapper = mapper;
        }


        /// <summary>
        /// Create a New Reservation
        /// </summary>
        [ProducesResponseType(201)]
        [HttpPost()]
        public async Task<IActionResult> CreateReservationAsync([FromBody] BikeReservationDto customerBikeReservation)
        {
            var bikeReservation = _mapper.Map<BikeReservationDto, BikeReservation>(customerBikeReservation);
            var operationResult = await _bikeReservationService.MakeReservationAsync(bikeReservation, bikeReservation.CustomerName,bikeReservation.CustomerId);

            if (operationResult.CompletedWithSuccess)
            {
                //await _bikeReservationMessagingService.PublishNewBikeReservationMessageAsync(operationResult.Result);
                return StatusCode(StatusCodes.Status201Created);
            }
            else
            {
                return BadRequest(operationResult.OperationError);
            }
        }
    }
}
