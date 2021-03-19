using System.Text.Json.Serialization;

namespace BikesIsland.Models.Messages
{
    public abstract class IntegrationMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
