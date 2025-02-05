using DataLayer.Context;
using DataLayer.Enums;
using DataLayer.Models.Data;
using DataLayer.Paystack.DTOs;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLayer.Service;
using ServiceLayer.Utils;

namespace ServiceLayer.Provider
{
    public class WebhookProcessor : IWebhookProcessor
    {
        private readonly ILogger<InstallmentService> _logger;
        private readonly IMSDbContext _context;
        private const string InstallmentCompletionJobId = "ba6138a1-7862-4c85-9e1a-89cc4b6f57fb";

        public WebhookProcessor(ILogger<InstallmentService> logger, IMSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task ProcessWebhookEventAsync(PaystackWebhookDto dto)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            var data = dto.GetData<ChargeWebhookDataDto>();

            try
            {
                if (dto.Event == Consts.PaystackSuccessEvent)
                {
                    var installment = await _context.Installments.Where(i => i.Id == data.Metadata.InstallmentId).FirstOrDefaultAsync();
                    if (installment == null) throw new Exception("Installment not found");

                    installment.Status = InstallmentStatus.Active;
                    await _context.SaveChangesAsync();

                    var amount = data.Amount / 100;
                    var payment = new Payment(PaymentMode.Momo, amount)
                    {
                        InstallmentId = data.Metadata.InstallmentId,
                        CreatedBy = Consts.SytemUser,
                        UpdatedBy = Consts.SytemUser
                    };
                    await _context.Payments.AddAsync(payment);
                    await _context.SaveChangesAsync();

                    var bill = await _context.Bills.Where(b => b.Id == data.Metadata.BillId).FirstOrDefaultAsync();
                    if (bill == null) throw new Exception("Bill not found");

                    bill.IsProcessed = true;
                    bill.Status = BillStatus.Paid;
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
                else
                {
                    var bill = await _context.Bills.Where(b => b.Id == data.Metadata.BillId).FirstOrDefaultAsync();
                    if (bill == null) throw new Exception("Bill not found");

                    bill.IsProcessed = true;
                    bill.Status = BillStatus.Overdue;
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "An error occured while processing callback");
            }
        }
    }
}
