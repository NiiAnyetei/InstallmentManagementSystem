using DataLayer.Paystack.DTOs;
using DataLayer.Context;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Flurl.Http;
using Flurl;
using Flurl.Http.Configuration;
using ServiceLayer.Utils;
using ServiceLayer.External.Paystack.Service;
using DataLayer.Models.Paystack.DTOs;
using DataLayer.Models.Data;
using DataLayer.Models.Paystack.Data;

namespace ServiceLayer.External.Paystack.Provider
{
    public class PaystackService : IPaystackService
    {
        private readonly ILogger<PaystackService> _logger;
        private readonly IMSDbContext _context;
        private readonly IConfiguration _config;

        public PaystackService(ILogger<PaystackService> logger, IMSDbContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        public async Task<PaystackResponse<CreateChargeResponseDto>> CreateCharge(CreateChargeDto dto)
        {
            try
            {
                dto.Amount = dto.Amount * 100;
                dto.Currency = Consts.Currency;

                const string url = Consts.ChargeUrl;

                var response = await GetClient().Request(url).PostJsonAsync(dto).ReceiveJson<PaystackResponse<CreateChargeResponseDto>>();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while charging customer");
                throw;
            }
        }

        public async Task<PaystackResponse<InitializeTransactionResponseDto>> InitializeTransaction(InitializePaymentDto dto)
        {
            try
            {
                var percentage = (decimal)2 / 100;
                var transactionFees = dto.Amount * percentage;
                var amount = (dto.Amount + transactionFees) * 100;

                dto.Amount = amount;
                dto.Currency = Consts.Currency;
                const string url = Consts.InitializeTransactionUrl;

                var response = await GetClient().Request(url).PostJsonAsync(dto).ReceiveJson<PaystackResponse<InitializeTransactionResponseDto>>();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while initializing transaction");
                throw;
            }
        }

        private IFlurlClient GetClient()
        {
            var baseUrl = _config["PaystackConfiguration:BaseUrl"];
            var token = _config["PaystackConfiguration:SecretKey"];

            var client = new FlurlClient(baseUrl);

            client.Headers.Clear();
            client.WithOAuthBearerToken(token);
            client.Headers.Add("Content-Type", "application/json");

            return client;
        }
    }
}
