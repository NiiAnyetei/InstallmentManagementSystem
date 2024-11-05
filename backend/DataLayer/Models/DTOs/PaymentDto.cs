using DataLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.DTOs;

public record NewPaymentDto([Required] string InstallmentId, [Required] PaymentMode PaymentMode, [Required] decimal Amount);

public record UpdatedPaymentDto(string? InstallmentId, PaymentMode? PaymentType, decimal? Amount) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(InstallmentId) && string.IsNullOrWhiteSpace(PaymentType.ToString()) && Amount is null or <= 0)
        {
            yield return new ValidationResult(
                $"At least one of the fields: Installment, {nameof(PaymentType)}, {nameof(Amount)} must be filled"
            );
        }
    }
};

public record PaymentDto(Guid Id, PaymentMode PaymentMode, decimal Amount, InstallmentDto Installment);

public record PaymentsDto(List<PaymentDto> Items, int Count);

public record PaymentsQuery(string? Customer, int Limit = 20, int Offset = 0);