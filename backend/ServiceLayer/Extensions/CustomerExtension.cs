using DataLayer.Models.Data;
using DataLayer.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Extensions
{
    public static class CustomerExtension
    {
        public static CustomerDto ToCustomerDto(this Customer customer)
        {
            return new CustomerDto(customer.Id, customer.FirstName, customer.LastName, $"{customer.FirstName} {customer.LastName}", customer.PhoneNumber, customer.Email, customer.IdentificationNumber);
        }

        public static Customer ToCustomer(this NewCustomerDto newCustomerDto)
        {
            return new Customer(newCustomerDto.FirstName, newCustomerDto.LastName, newCustomerDto.PhoneNumber, newCustomerDto.Email, newCustomerDto.IdentificationNumber);
        }
        
        public static Customer ToCustomer(this CustomerDto customerDto)
        {
            return new Customer(customerDto.FirstName, customerDto.LastName, customerDto.PhoneNumber, customerDto.Email, customerDto.IdentificationNumber);
        }
    }
}
