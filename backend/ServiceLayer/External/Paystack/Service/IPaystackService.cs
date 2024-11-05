using DataLayer.Models.Paystack.DTOs;
using DataLayer.Paystack.DTOs;

namespace ServiceLayer.External.Paystack.Service
{
    public interface IPaystackService
    {
        Task<PaystackResponse<CreateChargeResponseDto>> CreateCharge(CreateChargeDto dto);
        Task<PaystackResponse<InitializeTransactionResponseDto>> InitializeTransaction(InitializePaymentDto dto);
    }
}
