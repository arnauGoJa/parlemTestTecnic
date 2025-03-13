using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<Customer>> GetCustomerByIdAsync(string id, string partitionKey);
        Task<Result<CustomerProducts>> GetCustomerWithProductsAsync(string id, string partitionKey);
    }
}