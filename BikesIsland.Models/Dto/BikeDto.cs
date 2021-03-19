using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BikesIsland.Models.Dto
{
    public class BikeDto
    {
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("pricePerDay")]
        public decimal PricePerDay { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        public IFormFile image { get; set; }
    }
}
