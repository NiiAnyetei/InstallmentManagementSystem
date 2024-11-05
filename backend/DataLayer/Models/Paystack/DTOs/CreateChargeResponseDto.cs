using DataLayer.Models.Paystack.Data;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace DataLayer.Paystack.DTOs
{
    public class CreateChargeResponseDto
    {
        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonProperty("channel")]
        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonProperty("domain")]
        [JsonPropertyName("domain")]
        public string? Domain { get; set; }

        [JsonProperty("fees")]
        [JsonPropertyName("fees")]
        public long Fees { get; set; }

        [JsonProperty("gateway_response")]
        [JsonPropertyName("gateway_response")]
        public string? GatewayResponse { get; set; }

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonProperty("ip_address")]
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonProperty("paid_at")]
        [JsonPropertyName("paid_at")]
        public DateTimeOffset PaidAt { get; set; }

        [JsonProperty("reference")]
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonProperty("transaction_date")]
        [JsonPropertyName("transaction_date")]
        public DateTimeOffset TransactionDate { get; set; }

        [JsonProperty("authorization")]
        [JsonPropertyName("authorization")]
        public Authorization Authorization { get; set; } = new();

        [JsonProperty("customer")]
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; } = new();
    }
}
