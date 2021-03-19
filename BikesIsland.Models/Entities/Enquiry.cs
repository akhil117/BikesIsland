using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BikesIsland.Models.Entities
{
    public class Enquiry : BaseEntity 
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("customerAttachedEmail")]
        public string CustomerAttachedEmail { get; set; }
        [JsonPropertyName("attachmentUrl")]
        public string AttachmentUrl { get; set; }
    }
}
