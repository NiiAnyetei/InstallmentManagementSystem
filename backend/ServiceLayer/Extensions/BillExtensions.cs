using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Extensions
{
    public static class BillExtensions
    {
        public static Bill CreateCopy(this Bill bill)
        {
            return new Bill()
            {
                StartDate = bill.StartDate,
                DueDate = bill.DueDate,
                PaymentChannel = bill.PaymentChannel,
                CyclePeriod = bill.CyclePeriod
            };
        }

        public static BillDto ToBillDto(this Bill bill)
        {
            return new BillDto(
                bill.Id,
                bill.DueDate,
                bill.PaymentChannel,
                bill.Amount,
                bill.CyclePeriod,
                bill.Status,
                bill.Installment.ToInstallmentDto()
            );
        }
    }
}
