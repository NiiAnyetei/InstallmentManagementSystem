using DataLayer.Models.DTOs;
using DataLayer.Paystack.DTOs;
using Flurl.Http;
using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceLayer.External.AfricasTalking.Provider;
using ServiceLayer.Service;
using ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.External.Arkesel.Provider
{
    public class ArkeselSmsSender : ISmsSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ArkeselSmsSender> _logger;

        public ArkeselSmsSender(IConfiguration config, ILogger<ArkeselSmsSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<SendSmsResponse> SendSmsAsync(string to, string message)
        {
            try
            {
                const string url = Consts.ArkeselSmsUrl;
                var sender = _config["ArkeselConfiguration:From"];
                var recipients = new List<string>() { to };
                var smsDto = new ArkeselSmsDto(sender!, message, recipients);

                var response = await GetClient().Request(url).PostJsonAsync(smsDto).ReceiveJson<Response>();

                if(response.status == "success") {
                    return new SendSmsResponse() { IsSuccess = true };
                }
                else
                {
                    return new SendSmsResponse() { IsSuccess = false, ErrorMessage = response.message };
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
            var baseUrl = _config["ArkeselConfiguration:BaseUrl"];
            var apiKey = _config["ArkeselConfiguration:ApiKey"];

            var client = new FlurlClient(baseUrl);

            client.Headers.Clear();
            client.Headers.Add("api-key", apiKey);
            client.Headers.Add("Content-Type", "application/json");

            return client;
        }

        private sealed record ArkeselSmsDto(string sender, string message, List<string> recipients, bool sandbox = true);
        private sealed record Response(string status, string message);
    }
}
