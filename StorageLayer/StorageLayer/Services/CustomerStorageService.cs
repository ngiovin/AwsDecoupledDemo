using Common.Models;
using Common.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StorageLayer.Services
{
    public class CustomerStorageService
    {
        private List<Customer> _internalCustomer = new List<Customer>();
        private readonly CustomerContext _context;
        public CustomerStorageService(CustomerContext context)
        {
            _context = context;
            context.Database.EnsureCreated();
            
        }


        public async Task AddCustomers(List<Customer> customers)
        {
            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();
        }


        public async Task Truncate()
        {
            var all = await GetCustomers();
            _context.Customers.RemoveRange(all);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Customer>> GetCustomers()
        {
            return await Task.FromResult(_context.Customers.ToList());
        }

    }
}
