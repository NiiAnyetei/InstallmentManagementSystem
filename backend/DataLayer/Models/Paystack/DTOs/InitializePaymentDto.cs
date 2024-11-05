using DataLayer.Models.Paystack.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Models.Paystack.DTOs
{
    public record InitializePaymentDto
    {
        public InitializePaymentDto(string email, decimal amount, Metadata metadata)
        {
            Email = email;
            Amount = amount;
            Metadata = metadata;
        }

        [JsonProperty("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        
        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        //[JsonProperty("reference")]
        //[JsonPropertyName("reference")]
        //public string? Reference { get; set; }

        //[JsonProperty("callback_url")]
        //[JsonPropertyName("callback_url")]
        //public string? CallbackUrl { get; set; }
    }
}
