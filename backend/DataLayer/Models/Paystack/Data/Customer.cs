using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Models.Paystack.Data
{
    public class Customer
    {
        [JsonProperty("customer_code")]
        [JsonPropertyName("customer_code")]
        public string? CustomerCode { get; set; }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonProperty("risk_action")]
        [JsonPropertyName("risk_action")]
        public string? RiskAction { get; set; }
    }
}
