using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateAsync(NewCustomerDto customer, string username);
        Task<CustomerDto> GetAsync(Guid customerId);
        Task<CustomersDto> GetAllAsync(CustomersQuery query);
        Task<CustomerDto> UpdateAsync(Guid customerId, UpdatedCustomerDto customer, string username);
    }
}
