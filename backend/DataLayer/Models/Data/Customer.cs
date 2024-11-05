using DataLayer.Models.DTOs;
using Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.Data
{
    public class Customer(string firstName, string lastName, string phoneNumber, string email, string identificationNumber)
    {
        public Guid Id { get; set; } = SequentialGuidGenerator.Instance.Create();

        [MaxLength(100)]
        public string FirstName { get; set; } = firstName;

        [MaxLength(100)]
        public string LastName { get; set; } = lastName;

        [MaxLength(100)]
        public string FullName { get; set; } = $"{firstName} {lastName}";

        [MaxLength(100)]
        public string PhoneNumber { get; set; } = phoneNumber;
        [MaxLength(100)]
        public string Email { get; set; } = email;

        [MaxLength(100)]
        public string IdentificationNumber { get; set; } = identificationNumber;

        [MaxLength(100)]
        public string CreatedBy { get; set; } = default!;

        [MaxLength(100)]
        public string UpdatedBy { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Installment> Installment { get; set; } = new HashSet<Installment>();

        public void UpdateCustomer(UpdatedCustomerDto update, string username)
        {
            if (!string.IsNullOrWhiteSpace(update.FirstName)) FirstName = update.FirstName;
            if (!string.IsNullOrWhiteSpace(update.LastName)) LastName = update.LastName;
            if (!string.IsNullOrWhiteSpace(update.PhoneNumber)) PhoneNumber = update.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(update.Email)) Email = update.Email;
            if (!string.IsNullOrWhiteSpace(update.IdentificationNumber)) IdentificationNumber = update.IdentificationNumber;

            UpdatedBy = username;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
