using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Models.Paystack.DTOs
{
    public record InitializeTransactionResponseDto
    {
        [JsonProperty("authorization_url")]
        [JsonPropertyName("authorization_url")]
        public string? AuthorizationUrl { get; set; }

        [JsonProperty("access_code")]
        [JsonPropertyName("access_code")]
        public string? AccessCode { get; set; }

        [JsonProperty("reference")]
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }
    }
}
