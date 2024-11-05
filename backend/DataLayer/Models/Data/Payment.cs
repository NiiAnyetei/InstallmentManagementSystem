using DataLayer.Enums;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models.Data
{
    public class Payment(PaymentMode paymentMode, decimal amount)
    {
        public Guid Id { get; set; } = SequentialGuidGenerator.Instance.Create();

        public PaymentMode PaymentMode { get; set; } = paymentMode;
        public decimal Amount { get; set; } = amount;

        [MaxLength(100)]
        public string CreatedBy { get; set; } = default!;

        [MaxLength(100)]
        public string UpdatedBy { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Guid InstallmentId { get; set; }
        public Installment Installment{ get; set; } = null!;
    }
}
