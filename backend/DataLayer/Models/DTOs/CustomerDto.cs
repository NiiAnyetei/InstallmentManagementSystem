using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.DTOs;

public record NewCustomerDto([Required] string FirstName, [Required] string LastName, [Required] string PhoneNumber, [Required] string Email, [Required] string IdentificationNumber);

public record UpdatedCustomerDto(string? FirstName, string? LastName, string? PhoneNumber, string? Email, string? IdentificationNumber) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName) && string.IsNullOrWhiteSpace(PhoneNumber) && string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(IdentificationNumber))
        {
            yield return new ValidationResult(
                $"At least one of the fields: {nameof(FirstName)}, {nameof(LastName)}, {nameof(PhoneNumber)}, {nameof(Email)}, {nameof(IdentificationNumber)} must be filled"
            );
        }
    }
};

public record CustomerDto(Guid Id, string FirstName, string LastName, string FullName, string PhoneNumber, string Email, string IdentificationNumber);

public record CustomersDto(List<CustomerDto> Items, int Count);

public record CustomersQuery(string? FirstName, string? LastName, string? FullName, string? PhoneNumber, string? Email, int Limit = 20, int Offset = 0);