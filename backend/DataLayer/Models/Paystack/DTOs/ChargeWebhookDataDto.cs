using DataLayer.Models.DTOs;
using DataLayer.Models.Paystack.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Paystack.DTOs
{
    public class ChargeWebhookDataDto
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonProperty("domain")]
        [JsonPropertyName("domain")]
        public string? Domain { get; set; }

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonProperty("reference")]
        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        [JsonProperty("amount")]
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonProperty("gateway_response")]
        [JsonPropertyName("gateway_response")]
        public string? GatewayResponse { get; set; }

        [JsonProperty("paid_at")]
        [JsonPropertyName("paid_at")]
        public DateTimeOffset PaidAt { get; set; }

        [JsonProperty("created_at")]
        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("channel")]
        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonProperty("currency")]
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonProperty("ip_address")]
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }

        [JsonProperty("metadata")]
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; } = null!;

        [JsonProperty("log")]
        [JsonPropertyName("log")]
        public Log Log { get; set; } = new();

        [JsonProperty("fees")]
        [JsonPropertyName("fees")]
        public decimal Fees { get; set; }

        [JsonProperty("customer")]
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; } = new();

        [JsonProperty("authorization")]
        [JsonPropertyName("authorization")]
        public Authorization Authorization { get; set; } = new();

        public bool IsSuccessResponse { get => Status == "success"; }
    }

    
//public class Authorization
    //{
    //    [JsonProperty("authorization_code")]
    //    [JsonPropertyName("authorization_code")]
    //    public string? AuthorizationCode { get; set; }

    //    [JsonProperty("bin")]
    //    [JsonPropertyName("bin")]
    //    public string? Bin { get; set; }

    //    [JsonProperty("last4")]
    //    [JsonPropertyName("last4")]
    //    public string? Last4 { get; set; }

    //    [JsonProperty("exp_month")]
    //    [JsonPropertyName("exp_month")]
    //    public string? ExpMonth { get; set; }

    //    [JsonProperty("exp_year")]
    //    [JsonPropertyName("exp_year")]
    //    public string? ExpYear { get; set; }

    //    [JsonProperty("channel")]
    //    [JsonPropertyName("channel")]
    //    public string? Channel { get; set; }

    //    [JsonProperty("card_type")]
    //    [JsonPropertyName("card_type")]
    //    public string? CardType { get; set; }

    //    [JsonProperty("bank")]
    //    [JsonPropertyName("bank")]
    //    public string? Bank { get; set; }

    //    [JsonProperty("country_code")]
    //    [JsonPropertyName("country_code")]
    //    public string? CountryCode { get; set; }

    //    [JsonProperty("brand")]
    //    [JsonPropertyName("brand")]
    //    public string? Brand { get; set; }

    //    [JsonProperty("signature")]
    //    [JsonPropertyName("signature")]
    //    public string? Signature { get; set; }

    //    [JsonProperty("account_name")]
    //    [JsonPropertyName("account_name")]
    //    public string? AccountName { get; set; }
    //}

    //public class Customer
    //{
    //    [JsonProperty("id")]
    //    [JsonPropertyName("id")]
    //    public long Id { get; set; }

    //    [JsonProperty("first_name")]
    //    [JsonPropertyName("first_name")]
    //    public string? FirstName { get; set; }

    //    [JsonProperty("last_name")]
    //    [JsonPropertyName("last_name")]
    //    public string? LastName { get; set; }

    //    [JsonProperty("email")]
    //    [JsonPropertyName("email")]
    //    public string? Email { get; set; }

    //    [JsonProperty("customer_code")]
    //    [JsonPropertyName("customer_code")]
    //    public string? CustomerCode { get; set; }

    //    [JsonProperty("phone")]
    //    [JsonPropertyName("phone")]
    //    public string? Phone { get; set; }

    //    [JsonProperty("risk_action")]
    //    [JsonPropertyName("risk_action")]
    //    public string? RiskAction { get; set; }
    //}
    public class Log
    {
        [JsonProperty("time_spent")]
        [JsonPropertyName("time_spent")]
        public long TimeSpent { get; set; }

        [JsonProperty("attempts")]
        [JsonPropertyName("attempts")]
        public long Attempts { get; set; }

        [JsonProperty("authentication")]
        [JsonPropertyName("authentication")]
        public string? Authentication { get; set; }

        [JsonProperty("errors")]
        [JsonPropertyName("errors")]
        public long Errors { get; set; }

        [JsonProperty("success")]
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonProperty("mobile")]
        [JsonPropertyName("mobile")]
        public bool Mobile { get; set; }

        [JsonProperty("channel")]
        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonProperty("history")]
        [JsonPropertyName("history")]
        public List<History> History { get; set; } = new();
    }

    public class History
    {
        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonProperty("time")]
        [JsonPropertyName("time")]
        public long Time { get; set; }
    }

}
