using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Models.Paystack.Data
{
    public class Authorization
    {
        [JsonProperty("authorization_code")]
        [JsonPropertyName("authorization_code")]
        public string? AuthorizationCode { get; set; }

        [JsonProperty("bank")]
        [JsonPropertyName("bank")]
        public string? Bank { get; set; }

        [JsonProperty("bin")]
        [JsonPropertyName("bin")]
        public string? Bin { get; set; }

        [JsonProperty("brand")]
        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonProperty("channel")]
        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonProperty("country_code")]
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("exp_month")]
        [JsonPropertyName("exp_month")]
        public string? ExpMonth { get; set; }

        [JsonProperty("exp_year")]
        [JsonPropertyName("exp_year")]
        public string? ExpYear { get; set; }

        [JsonProperty("last4")]
        [JsonPropertyName("last4")]
        public string? Last4 { get; set; }

        [JsonProperty("reusable")]
        [JsonPropertyName("reusable")]
        public bool Reusable { get; set; }

        [JsonProperty("account_name")]
        [JsonPropertyName("account_name")]
        public string? AccountName { get; set; }
    }
}
