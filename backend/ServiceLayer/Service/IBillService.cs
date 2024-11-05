using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IBillService
    {
        Task<List<Bill>> CreateUpcomingBillsAsync(InstallmentDto installment);
        Task<BillDto> GetAsync(Guid billId);
        Task<BillsDto> GetAllAsync(BillsQuery query);
        Task<BillDto> UpdateAsync(Guid billId, UpdatedBillDto bill, string username);
        Task<List<Bill>> GetDueBillsAsync();
    }
}
