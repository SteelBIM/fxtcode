using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenPlatform.Application.Interfaces;
using OpenPlatform.Domain.Models;
using OpenPlatform.Domain.Repositories;

namespace OpenPlatform.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customer;

        public CustomerService(ICustomerRepository customer)
        {
            _customer = customer;
        }

        public Customer GetCustomer(string userId)
        {
            return _customer.GetCustomer(userId);
        }
    }
}
