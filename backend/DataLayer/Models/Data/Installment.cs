using DataLayer.Enums;
using DataLayer.Models.DTOs;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Data
{
    public class Installment(string item, string itemDetails, decimal amount, decimal initialDeposit, CyclePeriod cyclePeriod, int cycleNumber, string paymentChannel)
    {
        public Guid Id { get; set; } = SequentialGuidGenerator.Instance.Create();

        [MaxLength(100)]
        public string Item { get; set; } = item;
        public string ItemDetails { get; set; } = itemDetails;
        public decimal Amount { get; set; } = amount;
        public decimal InitialDeposit { get; set; } = initialDeposit;
        public decimal TotalAmountDue { get; set; } = amount - initialDeposit;
        public CyclePeriod CyclePeriod { get; set; } = cyclePeriod;
        public int CycleNumber { get; set; } = cycleNumber;
        public string PaymentChannel { get; set; } = paymentChannel;

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; } = default!;
        [MaxLength(100)]
        public string UpdatedBy { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public InstallmentStatus Status { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
        public ICollection<Bill> Bills { get; set; } = new HashSet<Bill>();

        public void UpdateInstallment(UpdatedInstallmentDto update, string username)
        {
            //if (!string.IsNullOrWhiteSpace(update.CustomerId)) CustomerId = Guid.Parse(update.CustomerId);
            if (!string.IsNullOrWhiteSpace(update.Item)) Item = update.Item;
            if (update.Amount > 0) TotalAmountDue = (decimal)(Amount - update.Amount);
            if (update.Amount > 0) Amount = (decimal)update.Amount;
            if (Amount <= 0) Status = InstallmentStatus.Inactive;

            UpdatedBy = username;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
