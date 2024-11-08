using DataLayer.Context;
using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceLayer.Extensions;
using ServiceLayer.Service;
using ServiceLayer.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Provider
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IMSDbContext _context;

        public CustomerService(ILogger<CustomerService> logger, IMSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<CustomerDto> CreateAsync(NewCustomerDto newCustomerDto, string username)
        {
            try
            {
                var newCustomer = newCustomerDto.ToCustomer();
                newCustomer.CreatedBy = username;
                newCustomer.UpdatedBy = username;

                await _context.Customers.AddAsync(newCustomer);
                await _context.SaveChangesAsync();
                var dto = newCustomer.ToCustomerDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating article");
                throw;
            }
        }

        public async Task<CustomerDto> GetAsync(Guid customerId)
        {
            try
            {
                var customer = await _context.Customers.AsNoTracking().Where(c => c.Id == customerId).FirstOrDefaultAsync();
                if (customer == null) throw new Exception("Customer not found");
                var dto = customer.ToCustomerDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching customer");
                throw;
            }
        }

        public async Task<CustomersDto> GetAllAsync(CustomersQuery query)
        {
            try
            {
                var customers = _context.Customers.Select(c => c);

                if (!string.IsNullOrWhiteSpace(query.FirstName)) customers = customers.Where(c => c.FirstName == query.FirstName);
                if (!string.IsNullOrWhiteSpace(query.LastName)) customers = customers.Where(c => c.LastName == query.LastName);
                if (!string.IsNullOrWhiteSpace(query.FullName)) customers = customers.Where(c => c.FullName.Contains(query.FullName));
                if (!string.IsNullOrWhiteSpace(query.PhoneNumber)) customers = customers.Where(c => c.PhoneNumber == query.PhoneNumber);
                if (!string.IsNullOrWhiteSpace(query.Email)) customers = customers.Where(c => c.Email == query.Email);

                var total = await customers.CountAsync();
                var pageQuery = customers.Skip(query.Offset).Take(query.Limit).AsNoTracking();
                var page = await pageQuery.ToListAsync();

                var customersDto = new List<CustomerDto>();

                foreach (var customer in page)
                {
                    customersDto.Add(customer.ToCustomerDto());
                }

                return new CustomersDto(customersDto, total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching customers");
                throw;
            }
        }

        public async Task<CustomerDto> UpdateAsync(Guid customerId, UpdatedCustomerDto updatedCustomerDto, string username)
        {
            try
            {
                var customer = await _context.Customers.Where(c => c.Id == customerId).FirstOrDefaultAsync();

                if (customer == null) throw new Exception("Customer not found");

                customer.UpdateCustomer(updatedCustomerDto, username);
                await _context.SaveChangesAsync();
                var dto = customer.ToCustomerDto();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while fetching articles");
                throw;
            }
        }
    }
}
