using DataLayer.Context;
using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLayer.Extensions;
using ServiceLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Provider
{
    public class PaymentService : IPaymentService
    {
        private readonly IMSDbContext _context;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IMSDbContext context, ILogger<PaymentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<PaymentDto> CreateAsync(NewPaymentDto payment, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentsDto> GetAllAsync(PaymentsQuery query)
        {
            try
            {
                var payments = _context.Payments.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(query.Customer)) payments = payments.Where(p => p.Installment.Customer.FullName.Contains(query.Customer));

                var total = await payments.CountAsync();
                var pageQuery = payments.Include(p => p.Installment).ThenInclude(i => i.Customer).Skip(query.Offset).Take(query.Limit).AsNoTracking();
                var page = await pageQuery.Select(p => new PaymentDto(p.Id, p.PaymentMode, p.Amount, p.Installment.ToInstallmentDto())).ToListAsync();

                return new PaymentsDto(page, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching payments");
                throw;
            }
        }

        public async Task<PaymentDto> GetAsync(Guid paymentId)
        {
            try
            {
                var payment = await _context.Payments.AsNoTracking().Include(p => p.Installment).ThenInclude(i => i.Customer).Where(p => p.Id == paymentId).FirstOrDefaultAsync();
                if (payment == null) throw new Exception("Payment not found");
                var dto = payment.ToPaymentDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching payment");
                throw;
            }
        }

        public Task<PaymentDto> UpdateAsync(Guid paymentId, UpdatedPaymentDto payment, string username)
        {
            throw new NotImplementedException();
        }
    }
}
