using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        Task<Result<CustomerProducts>> GetCustomerWithProductsAsync(string customerId, string partitionKey);
    }

}
