using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using DataLayer.Models.Paystack.Data;
using Newtonsoft.Json;

namespace DataLayer.Paystack.DTOs
{
    public record CreateChargeDto
    {
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; } = null!;

        [JsonProperty("mobile_money")]
        [JsonPropertyName("mobile_money")]
        public MobileMoney MobileMoney { get; set; } = new();
    }

    public partial record MobileMoney
    {
        [JsonProperty("phone")]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonProperty("provider")]
        [JsonPropertyName("provider")]
        public string? Provider { get; set; }
    }
}
