using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreateAsync(NewPaymentDto payment, string username);
        Task<PaymentDto> GetAsync(Guid paymentId);
        Task<PaymentsDto> GetAllAsync(PaymentsQuery query);
        Task<PaymentDto> UpdateAsync(Guid paymentId, UpdatedPaymentDto payment, string username);
    }
}
