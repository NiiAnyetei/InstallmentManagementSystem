using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.Paystack.DTOs
{
    public record PaystackWebhookDto
    {
        [JsonProperty("event")]
        [JsonPropertyName("event")]
        public string Event { get; set; } = string.Empty;

        [JsonProperty("data")]
        [JsonPropertyName("data")]
        public object Data { get; set; } = new();

        public T GetData<T>()
            where T : class, new()
        {
            var deserializedData = System.Text.Json.JsonSerializer.Serialize(Data, typeof(object));
            return System.Text.Json.JsonSerializer.Deserialize<T>(deserializedData) ?? new T();
        }
    }
}
