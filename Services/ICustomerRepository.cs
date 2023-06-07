using CoraetionTask.Data.Base;
using CoraetionTask.Models;

namespace CoraetionTask.Services
{
    public interface ICustomerRepository : IEntityBaseRepository<Customer> {
        Task AddProductToCustomer(int customerId, Product product);

        Task<Customer> GetCustomerByFullName(string customerFullName);

        Task<Customer> GetCustomerByEmailAsync(string customerEmail);



    }

}
