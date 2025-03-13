using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;

namespace Core
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerValidator _customerValidator;

        public CustomerService(ICustomerRepository customerRepository, ICustomerValidator customerValidator)
        {
            _customerRepository = customerRepository;
            _customerValidator = customerValidator;
        }

        public async Task<Result<Customer>> GetCustomerByIdAsync(string id, string partitionKey)
        {
            Result<Customer> customerResult;
            var validationResult = _customerValidator.ValidateCustomerId(id, partitionKey);
            if (validationResult.Success)
            {
                customerResult = await GetCustomerById(id, partitionKey);
            }
            else
            {
                customerResult = Result<Customer>.Fail(validationResult.Message);
            }

            return customerResult;
        }


        public async Task<Result<CustomerProducts>> GetCustomerWithProductsAsync(string id, string partitionKey)
        {
            Result<CustomerProducts> customerResult;
            var validationResult = _customerValidator.ValidateCustomerId(id, partitionKey);
            if (validationResult.Success)
            {
                customerResult = await GetCustomerWithProducts(id, partitionKey);
            }
            else
            {
                customerResult = Result<CustomerProducts>.Fail(validationResult.Message);
            }

            return customerResult;
        }

        private async Task<Result<CustomerProducts>> GetCustomerWithProducts(string id, string partitionKey)
        {
            Result<CustomerProducts> customerResult = await _customerRepository.GetCustomerWithProductsAsync(id, partitionKey);

            if (customerResult.Success is false)
            {
                customerResult = Result<CustomerProducts>.Fail("Customer not found");
            }
            return customerResult;
        }

        private async Task<Result<Customer>> GetCustomerById(string id, string partitionKey)
        {
            Result<Customer> customerResult = await _customerRepository.GetByIdAsync(id, partitionKey);
            if (customerResult.Success is false)
            {
                customerResult = Result<Customer>.Fail("Customer not found");
            }

            return customerResult;
        }


    }

}
