using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BikesIsland.Models.Entities
{
   public abstract class BaseEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
