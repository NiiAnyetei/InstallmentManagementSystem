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
using System.Linq;

namespace ServiceLayer.Provider
{
    public class InstallmentService : IInstallmentService
    {
        private readonly ILogger<InstallmentService> _logger;
        private readonly IMSDbContext _context;

        public InstallmentService(ILogger<InstallmentService> logger, IMSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<NewInstallmentDto> CreateAsync(NewInstallmentDto installment, string username)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var activeCustomerInstallment = await _context.Installments.AsNoTracking().Where(i => i.CustomerId == Guid.Parse(installment.CustomerId) && i.Status == InstallmentStatus.Active).FirstOrDefaultAsync();

                if (activeCustomerInstallment is not null) throw new Exception("Customer already has an installment plan");

                //create installment
                var newInstallment = installment.ToInstallment();
                newInstallment.CustomerId = Guid.Parse(installment.CustomerId);
                newInstallment.StartDate = DateTime.UtcNow.Date;
                newInstallment.EndDate = GetEndDate(newInstallment);
                newInstallment.CreatedBy = username;
                newInstallment.UpdatedBy = username;
                newInstallment.Status = InstallmentStatus.Active;

                await _context.Installments.AddAsync(newInstallment);
                await _context.SaveChangesAsync();

                //create bills
                var currentBill = newInstallment.ToBill();
                var installmentId = newInstallment.Id;
                var amountDue = decimal.Round(newInstallment.TotalAmountDue / newInstallment.CycleNumber);

                currentBill.InstallmentId = installmentId;
                currentBill.Amount = amountDue;
                currentBill.IsProcessed = false;
                currentBill.Status = BillStatus.Pending;

                var bills = new List<Bill>();

                for (int i = 0; i < installment.CycleNumber; i++)
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

                transaction.Commit();

                var dto = newInstallment.ToNewInstallmentDto();
                return dto;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "An error occured while creating installment");
                throw;
            }
        }

        public async Task<InstallmentsDto> GetAllAsync(InstallmentsQuery query)
        {
            try
            {
                var installments = _context.Installments.Select(c => c);

                if (!string.IsNullOrWhiteSpace(query.Item)) installments = installments.Where(i => i.Item.Contains(query.Item));
                if (!string.IsNullOrWhiteSpace(query.Customer)) installments = installments.Where(i => i.Customer.FullName.Contains(query.Customer));
                if (query.From.HasValue) installments = installments.Where(i => i.StartDate >= query.From);
                if (query.To.HasValue) installments = installments.Where(i => i.StartDate <= query.To);
                if (query.Status.HasValue) installments = installments.Where(i => i.Status == query.Status);

                var total = await installments.CountAsync();
                var pageQuery = installments.OrderByDescending(i => i.CreatedAt).Skip(query.Offset).Take(query.Limit).AsNoTracking();
                var page = await pageQuery.Select(i => new InstallmentDto(i.Id, i.Customer.ToCustomerDto(), i.Item, i.ItemDetails, i.Amount, i.InitialDeposit, i.TotalAmountDue, i.CyclePeriod, i.CycleNumber, i.PaymentChannel, i.StartDate, i.EndDate, i.CreatedBy, i.Status)).ToListAsync();

                return new InstallmentsDto(page, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching installments");
                throw;
            }
        }

        public async Task<InstallmentDto> GetAsync(Guid installmentId)
        {
            try
            {
                var installment = await _context.Installments.AsNoTracking().Include(i => i.Customer).Where(c => c.Id == installmentId).FirstOrDefaultAsync();
                if (installment == null) throw new Exception("Installment not found");
                var dto = installment.ToInstallmentDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching installment");
                throw;
            }
        }

        public Task<InstallmentDto> UpdateAsync(Guid installmentId, UpdatedInstallmentDto installment, string username)
        {
            throw new NotImplementedException();
        }

        public async Task CompleteInstallmentsAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var installments = await _context.Installments.Include(i => i.Bills).Where(i => i.Status == InstallmentStatus.Active).ToListAsync();
                var completedInstallments = new List<Installment>();

                foreach (var installment in installments)
                {
                    var complete = installment.Bills.All(i => i.IsProcessed && i.Status == BillStatus.Paid);

                    if (complete)
                    {
                        installment.Status = InstallmentStatus.Paid;
                        completedInstallments.Add(installment);
                    }
                }

                await _context.BulkUpdateAsync(completedInstallments);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "An error occured while completing installment");
                throw;
            }
        }

        private DateTimeOffset GetEndDate(Installment newInstallment)
        {
            var startDate = newInstallment.StartDate;
            var days = newInstallment.CycleNumber * (int)newInstallment.CyclePeriod;
            var endDate = startDate.AddDays(days);
            return endDate;
        }
    }
}
