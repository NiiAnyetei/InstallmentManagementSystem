using DataLayer.Enums;
using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Extensions
{
    public static class PaymentExtensions
    {
        public static PaymentDto ToPaymentDto(this Payment payment)
        {
            return new PaymentDto(
                payment.Id,
                payment.PaymentMode,
                payment.CreatedAt,
                payment.Amount,
                payment.Installment.ToInstallmentDto()
            );
        }
    }
}
