
using CoraetionTask.Data;
using CoraetionTask.Data.Base;
using CoraetionTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CoraetionTask.Services
{
    public class CustomerRepository : EntityBaseRepository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext context;
        public CustomerRepository(AppDbContext db) : base(db)
        {

            context = db;

        }

        public async Task AddProductToCustomer(int customerId, Product product)
        {
            Customer customer = await GetByIDAsync(customerId);

            if (customer != null)
            {
                customer.Products.Add(product);

                await context.SaveChangesAsync(); 
            }
        }

        public async Task<Customer> GetCustomerByFullName(string customerFullName)
        {
            Customer customer = await context.Customers.FirstOrDefaultAsync(c=> c.FullName== customerFullName);

            return customer;
        }
    }
}
