using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BikesIsland.Models.Messages
{
    public class BikeReservationIntegrationMessage : IntegrationMessage
    {
        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }
        [JsonPropertyName("bikeId")]
        public string BikeId { get; set; }
        [JsonPropertyName("rentFrom")]
        public DateTime RentFrom { get; set; }
        [JsonPropertyName("rentTo")]
        public DateTime RentTo { get; set; }
    }
}
