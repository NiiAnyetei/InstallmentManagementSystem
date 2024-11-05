using DataLayer.Enums;
using Shared.Utils;

namespace DataLayer.Models.Data
{
    public class Bill()
    {
        public Guid Id { get; set; } = SequentialGuidGenerator.Instance.Create();
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public string PaymentChannel { get; set; } = default!;
        public decimal Amount { get; set; }
        public CyclePeriod CyclePeriod { get; set; }
        public bool IsProcessed { get; set; } = false;
        public DateTimeOffset? ProcessedAt { get; set; }
        public BillStatus Status { get; set; }

        public Guid InstallmentId { get; set; }
        public Installment Installment { get; set; } = null!;
    }
}
