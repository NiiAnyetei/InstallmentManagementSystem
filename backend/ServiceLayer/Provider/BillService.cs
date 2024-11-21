using DataLayer.Context;
using DataLayer.Enums;
using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLayer.Extensions;
using ServiceLayer.Service;
using Shared.Utils;

namespace ServiceLayer.Provider
{
    public class BillService : IBillService
    {
        private readonly ILogger<BillService> _logger;
        private readonly IMSDbContext _context;

        public BillService(ILogger<BillService> logger, IMSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Bill>> CreateUpcomingBillsAsync(InstallmentDto installment)
        {
            try
            {
                var currentBill = installment.ToBill();
                var installmentId = installment.Id;
                var amountDue = installment.TotalAmountDue / installment.CycleNumber;

                currentBill.InstallmentId = installmentId;
                currentBill.Amount = amountDue;
                currentBill.Status = BillStatus.Pending;

                var bills = new List<Bill>();

                for (int i = 1; i < installment.CycleNumber; i++)
                {
                    // Clone current Bill to get next Bill in series from Cycle period.
                    var nextBill = currentBill.CreateCopy();

                    nextBill.Id = SequentialGuidGenerator.Instance.Create();
                    nextBill.InstallmentId = installmentId;
                    nextBill.Amount = amountDue;
                    nextBill.Status = BillStatus.Pending;

                    // Increment the DueDate by the selected cycle period.
                    var cyclePeriod = (int)currentBill.CyclePeriod;
                    nextBill.DueDate = currentBill.DueDate.AddDays(cyclePeriod);

                    // Add to List and set as current Bill to be used to get next StartDate
                    bills.Add(nextBill);
                    currentBill = nextBill;
                }

                await _context.BulkInsertAsync(bills);

                return bills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating bills");
                throw;
            }
        }

        public async Task<BillsDto> GetAllAsync(BillsQuery query)
        {
            try
            {
                var bills = _context.Bills.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(query.Customer)) bills = bills.Where(b => b.Installment.Customer.FullName.Contains(query.Customer));
                if (query.From.HasValue) bills = bills.Where(p => p.DueDate >= query.From);
                if (query.To.HasValue) bills = bills.Where(p => p.DueDate <= query.To);
                if (query.Status.HasValue) bills = bills.Where(p => p.Status <= query.Status);

                var total = await bills.CountAsync();
                var pageQuery = bills.Include(b => b.Installment).ThenInclude(i => i.Customer).Skip(query.Offset).Take(query.Limit).AsNoTracking();
                var page = await pageQuery.Select(b => new BillDto(b.Id, b.DueDate, b.PaymentChannel, b.Amount, b.CyclePeriod, b.Status, b.Installment.ToInstallmentDto())).ToListAsync();

                return new BillsDto(page, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching bills");
                throw;
            }
        }

        public async Task<BillDto> GetAsync(Guid billId)
        {
            try
            {
                var bill = await _context.Bills.AsNoTracking().Include(b => b.Installment).ThenInclude(i => i.Customer).Where(b => b.Id == billId).FirstOrDefaultAsync();

                if (bill == null) throw new Exception("Bill not found");
                var dto = bill.ToBillDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching bill");
                throw;
            }
        }

        public Task<BillDto> UpdateAsync(Guid billId, UpdatedBillDto bill, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Bill>> GetDueBillsAsync()
        {
            try
            {
                var bills = await _context.Bills.AsNoTracking().Include(b => b.Installment).ThenInclude(i => i.Customer).Where(b => b.DueDate.Date <= DateTime.UtcNow.Date && b.IsProcessed == false && b.Status != BillStatus.Paid).ToListAsync();

                return bills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching due bills");
                throw;
            }
        }
    }
}
