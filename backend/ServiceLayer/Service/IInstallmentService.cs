using DataLayer.Models.Data;
using DataLayer.Models.DTOs;

namespace ServiceLayer.Service
{
    public interface IInstallmentService
    {
        Task<NewInstallmentDto> CreateAsync(NewInstallmentDto installment, string username);
        Task<InstallmentDto> GetAsync(Guid installmentId);
        Task<InstallmentsDto> GetAllAsync(InstallmentsQuery query);
        Task<NewInstallmentDto> UpdateAsync(Guid installmentId, UpdatedInstallmentDto installment, string username);
        Task<InstallmentDto> DeleteAsync(Guid installmentId, string username);
        Task CompleteInstallmentsAsync();
    }
}
