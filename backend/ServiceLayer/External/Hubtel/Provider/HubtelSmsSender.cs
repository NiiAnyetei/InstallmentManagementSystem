using DataLayer.Models.DTOs;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLayer.External.Hubtel.Provider;
using ServiceLayer.Service;
using ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceLayer.External.Hubtel.Provider
{
    public class HubtelSmsSender : ISmsSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HubtelSmsSender> _logger;

        public HubtelSmsSender(IConfiguration config, ILogger<HubtelSmsSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<SendSmsResponse> SendSmsAsync(string to, string message)
        {
            try
            {
                const string url = Consts.HubtelSmsUrl;
                var from = _config["HubtelConfiguration:From"];
                var smsDto = new HubtelSmsDto(from!, to, message);

                var response = await GetClient().Request(url).PostJsonAsync(smsDto).ReceiveJson<Response>();

                if (response.Status == 0)
                {
                    return new SendSmsResponse() { IsSuccess = true };
                }
                else
                {
                    return new SendSmsResponse() { IsSuccess = false, ErrorMessage = string.Format("SMS sending response with status: {0}", response.Status) };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while sending SMS");
                throw;
            }
        }

        private IFlurlClient GetClient()
        {
            var baseUrl = _config["HubtelConfiguration:BaseUrl"];
            var username = _config["HubtelConfiguration:ClientID"];
            var password = _config["HubtelConfiguration:ClientSecret"];

            var client = new FlurlClient(baseUrl);
            client.Headers.Clear();
            client.WithBasicAuth(username, password);
            client.Headers.Add("Accept", "application/json");
            client.Headers.Add("Content-Type", "application/json");

            return client;
        }

        private sealed record HubtelSmsDto(string from, string to, string content);

        //public sealed record Response
        //{
        //    [JsonProperty("message")]
        //    [JsonPropertyName("message")]
        //    public string? Message { get; set; }

        //    [JsonProperty("responseCode")]
        //    [JsonPropertyName("responseCode")]
        //    public string? ResponseCode { get; set; }

        //    [JsonProperty("data")]
        //    [JsonPropertyName("data")]
        //    public Data Data { get; set; } = new();
        //}

        public sealed record Response
        {
            [JsonProperty("rate")]
            [JsonPropertyName("rate")]
            public decimal Rate { get; set; }

            [JsonProperty("messageId")]
            [JsonPropertyName("messageId")]
            public string? MessageId { get; set; }

            [JsonProperty("status")]
            [JsonPropertyName("status")]
            public long? Status { get; set; }

            [JsonProperty("networkId")]
            [JsonPropertyName("networkId")]
            public string? NetworkId { get; set; }
        }
    }
}
