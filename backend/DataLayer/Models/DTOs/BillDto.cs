using DataLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.DTOs
{
    public record NewBillDto([Required] string InstallmentId, [Required] DateTimeOffset DueDate, [Required] string PaymentChannel, [Required] decimal Amount, [Required] CyclePeriod CyclePeriod, [Required] BillStatus BillStatus);

    public record UpdatedBillDto(string InstallmentId, DateTimeOffset DueDate, string PaymentChannel, decimal Amount, CyclePeriod CyclePeriod, BillStatus Status) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(InstallmentId))
            {
                yield return new ValidationResult(
                    $"At least one of the fields: Installment must be filled"
                );
            }
        }
    };

    public record BillDto(Guid Id, DateTimeOffset DueDate, string PaymentChannel, decimal Amount, CyclePeriod CyclePeriod, BillStatus Status, bool IsProcessed, InstallmentDto Installment);

    public record BillsDto(List<BillDto> Items, int Count);

    public record BillsQuery(string? Customer, DateTime? From, DateTime? To, BillStatus? Status, int Limit = 20, int Offset = 0);
}
