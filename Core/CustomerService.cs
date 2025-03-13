using Domain.Entities;
using Infrastructure;

namespace Core
{
    public class CustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer?> GetCustomerByIdAsync(string id, string partitionKey)
        {
            return await _customerRepository.GetByIdAsync(id, partitionKey);
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
