using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BikesIsland.Models.Entities
{
    public class BikeReservation : BaseEntity
    {
        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }
        [JsonPropertyName("bikeId")]
        public string BikeId { get; set; }
        [JsonPropertyName("rentFrom")] //apudu nunchi
        public DateTime RentFrom { get; set; } //apudu varaku
        [JsonPropertyName("rentTo")]
        public DateTime RentTo { get; set; }
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; }
    }
}
