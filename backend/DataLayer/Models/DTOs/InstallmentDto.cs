using DataLayer.Enums;
using DataLayer.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.DTOs;

public record NewInstallmentDto(
    [Required] string CustomerId,
    [Required] string Item,
    [Required] string ItemDetails,
    [Required] decimal Amount,
    [Required] decimal InitialDeposit,
    [Required] CyclePeriod CyclePeriod,
    [Required] int CycleNumber,
    [Required] string PaymentChannel,
    bool ProcessInitialDeposit = false
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Amount is 0)
        {
            yield return new ValidationResult(
                $"{nameof(Amount)} must be filled"
            );
        }
        
        if (ProcessInitialDeposit && InitialDeposit <= 0)
        {
            yield return new ValidationResult(
                $"{nameof(InitialDeposit)} must be filled"
            );
        }
        
        if (InitialDeposit > Amount)
        {
            yield return new ValidationResult(
                $"{nameof(InitialDeposit)} cannot be more than {nameof(Amount)}"
            );
        }

        if (CycleNumber is 0)
        {
            yield return new ValidationResult(
                $"{nameof(CycleNumber)} must be filled"
            );
        }
    }
}

public record UpdatedInstallmentDto(
    string? CustomerId,
    string? Item,
    string? ItemDetails,
    decimal? Amount,
    decimal? InitialDeposit,
    CyclePeriod? CyclePeriod,
    int? CycleNumber,
    string? PaymentChannel
    ) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (
            string.IsNullOrWhiteSpace(CustomerId) && 
            string.IsNullOrWhiteSpace(Item) && 
            string.IsNullOrWhiteSpace(ItemDetails) && 
            Amount is null or 0 && 
            InitialDeposit is null && 
            string.IsNullOrWhiteSpace(CyclePeriod.ToString()) &&
            CycleNumber is null or 0 &&
            string.IsNullOrWhiteSpace(PaymentChannel)
            )
        {
            yield return new ValidationResult(
                $"At least one of the fields: {nameof(CustomerId)}, {nameof(Item)}, {nameof(ItemDetails)}, {nameof(Amount)}, {nameof(InitialDeposit)}, {nameof(CyclePeriod)}, {nameof(CycleNumber)}, {nameof(PaymentChannel)} must be filled"
            );
        }
    }
};

public record InstallmentDto(
    Guid Id,
    CustomerDto Customer,
    string Item,
    string ItemDetails,
    decimal Amount,
    decimal InitialDeposit,
    decimal TotalAmountDue,
    CyclePeriod CyclePeriod,
    int CycleNumber,
    string PaymentChannel,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string CreatedBy,
    InstallmentStatus Status
);

public record InstallmentsDto(List<InstallmentDto> Items, int Count);

public record InstallmentsQuery(string? Item, string? Customer, DateTime? From, DateTime? To, InstallmentStatus? Status, int Limit = 20, int Offset = 0);