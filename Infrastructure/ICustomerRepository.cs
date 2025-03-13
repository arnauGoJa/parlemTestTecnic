using Domain.Entities;
using Domain.Models;

namespace Infrastructure
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Task<CustomerProducts> GetCustomerWithProductsAsync(string customerId, string partitionKey);
    }

}
