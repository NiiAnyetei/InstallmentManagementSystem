using DataLayer.Models.DTOs;
using DataLayer.Paystack.DTOs;
using Flurl.Http;
using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceLayer.External.Paystack.Provider;
using ServiceLayer.Service;
using ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.External.AfricasTalking.Provider
{
    public class AfricasTalkingSmsSender : ISmsSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AfricasTalkingSmsSender> _logger;

        public AfricasTalkingSmsSender(IConfiguration config, ILogger<AfricasTalkingSmsSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<SendSmsResponse> SendSmsAsync(string to, string message)
        {
            try
            {
                const string url = Consts.AfricasTalkingSmsUrl;
                var username = _config["AfricasTalkingConfiguration:Username"];
                var from = _config["AfricasTalkingConfiguration:From"];

                var smsDto = new AfricasTalkingSmsDto(username!, to, message);
                //var smsDto = new AfricasTalkingSmsDto(username!, to, message, from!);
                var response = await GetClient().Request(url).PostUrlEncodedAsync(smsDto);
                var sendMessageResponse = await response.GetJsonAsync<Response>();

                var unsuccessfulRecipients = sendMessageResponse.SmsMessageData.Recipients.Where(m => m.Status != "Success").ToList();
                if (unsuccessfulRecipients.Count == 0 && sendMessageResponse.SmsMessageData.Message != "InvalidSenderId")
                {
                    return new SendSmsResponse() { IsSuccess = true };
                }
                else
                {
                    StringBuilder sb = new("Sms failed to send to the following number(s)");
                    foreach (var recipient in unsuccessfulRecipients)
                    {
                        sb.Append(recipient.Number).Append(" : ").AppendLine(recipient.Status);
                    }

                    return new SendSmsResponse() { IsSuccess = false, ErrorMessage = sb.ToString() };
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
            var baseUrl = _config["AfricasTalkingConfiguration:BaseUrl"];
            var apiKey = _config["AfricasTalkingConfiguration:ApiKey"];

            var client = new FlurlClient(baseUrl);

            client.Headers.Clear();
            client.Headers.Add("apiKey", apiKey);
            client.Headers.Add("Accept", "application/json");

            return client;
        }

        private sealed record AfricasTalkingSmsDto(string username, string to, string message);
        //private sealed record AfricasTalkingSmsDto(string username, string to, string message, string from);
        private sealed record Response
        {
            [JsonProperty("SMSMessageData")]
            public SmsMessageData SmsMessageData { get; set; } = new();
        }

        private sealed record SmsMessageData
        {
            [JsonProperty("Message")]
            public string? Message { get; set; }

            [JsonProperty("Recipients")]
            public List<Recipient> Recipients { get; set; } = new();
        }

        private sealed record Recipient
        {
            [JsonProperty("statusCode")]
            public long? StatusCode { get; set; }

            [JsonProperty("number")]
            public string? Number { get; set; }

            [JsonProperty("status")]
            public string? Status { get; set; }

            [JsonProperty("cost")]
            public string? Cost { get; set; }

            [JsonProperty("messageId")]
            public string? MessageId { get; set; }
        }
    }
}
