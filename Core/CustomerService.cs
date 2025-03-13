using Domain.Entities;
using Domain.Models;
using Infrastructure;

namespace Core
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id, string partitionKey)
        {
            return await _customerRepository.GetByIdAsync(id, partitionKey);
        }
        public async Task<CustomerProducts> GetCustomerWithProductsAsync(string customerId, string partitionKey)
        {
            return await _customerRepository.GetCustomerWithProductsAsync(customerId, partitionKey);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(string partitionKey)
        {
            return await _customerRepository.GetAllAsync(partitionKey);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _customerRepository.AddAsync(customer);
        }
    }

}
