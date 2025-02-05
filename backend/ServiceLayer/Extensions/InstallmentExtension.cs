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
    public static class InstallmentExtension
    {
        public static NewInstallmentDto ToNewInstallmentDto(this Installment installment)
        {
            return new NewInstallmentDto(
                installment.CustomerId.ToString(),
                installment.Item,
                installment.ItemDetails,
                installment.Amount,
                installment.InitialDeposit,
                installment.CyclePeriod,
                installment.CycleNumber,
                installment.PaymentChannel
            );
        }
        
        public static NewInstallmentDto ToNewInstallmentDto(this UpdatedInstallmentDto updatedInstallmentDto)
        {
            return new NewInstallmentDto(
                updatedInstallmentDto.CustomerId.ToString(),
                updatedInstallmentDto.Item,
                updatedInstallmentDto.ItemDetails,
                (decimal)updatedInstallmentDto.Amount,
                (decimal)updatedInstallmentDto.InitialDeposit,
                (CyclePeriod)updatedInstallmentDto.CyclePeriod,
                (int)updatedInstallmentDto.CycleNumber,
                updatedInstallmentDto.PaymentChannel
            );
        }

        public static Installment ToInstallment(this NewInstallmentDto newInstallmentDto)
        {
            return new Installment(
                newInstallmentDto.Item,
                newInstallmentDto.ItemDetails,
                newInstallmentDto.Amount,
                newInstallmentDto.InitialDeposit,
                newInstallmentDto.CyclePeriod,
                newInstallmentDto.CycleNumber,
                newInstallmentDto.PaymentChannel
            );
        }
        
        public static Installment ToInstallment(this UpdatedInstallmentDto updatedInstallmentDto)
        {
            return new Installment(
                updatedInstallmentDto.Item,
                updatedInstallmentDto.ItemDetails,
                (decimal)updatedInstallmentDto.Amount,
                (decimal)updatedInstallmentDto.InitialDeposit,
                (CyclePeriod)updatedInstallmentDto.CyclePeriod,
                (int)updatedInstallmentDto.CycleNumber,
                updatedInstallmentDto.PaymentChannel
            );
        }

        public static InstallmentDto ToInstallmentDto(this Installment installment)
        {
            return new InstallmentDto(
                installment.Id,
                installment.Customer.ToCustomerDto(),
                installment.Item,
                installment.ItemDetails,
                installment.Amount,
                installment.InitialDeposit,
                installment.TotalAmountDue,
                installment.CyclePeriod,
                installment.CycleNumber,
                installment.PaymentChannel,
                installment.StartDate,
                installment.EndDate,
                installment.CreatedBy,
                installment.Status
            );
        }

        public static Bill ToBill(this InstallmentDto installment)
        {
            return new Bill()
            {
                StartDate = installment.StartDate,
                DueDate = installment.StartDate,
                PaymentChannel = installment.PaymentChannel,
                CyclePeriod = installment.CyclePeriod
            };
        }

        public static Bill ToBill(this Installment installment)
        {
            return new Bill()
            {
                StartDate = installment.StartDate,
                DueDate = installment.StartDate,
                PaymentChannel = installment.PaymentChannel,
                CyclePeriod = installment.CyclePeriod
            };
        }
    }
}
