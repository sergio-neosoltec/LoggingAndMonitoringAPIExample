﻿using LoggingAndMonitoringAPIExample.Logic.Entities;
using LoggingAndMonitoringAPIExample.Logic.Models;
using LoggingAndMonitoringAPIExample.Logic.Parameters;

namespace LoggingAndMonitoringAPIExample.Logic.Services
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetAllCustomersAsync(CustomerResourceParameters customerResourceParameters);
        public Task<Customer> CreateCustomerAsync(Customer customer);
        public Task<Customer?> GetCustomerAsync(int customerId);
        public Task<bool> CustomerExistsAsync(int customerId);
    }
}
