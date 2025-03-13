using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : CosmosRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CosmosClient cosmosClient, string databaseName, string containerName)
            : base(cosmosClient, databaseName, containerName) { }


        public async Task<Result<CustomerProducts>> GetCustomerWithProductsAsync(string id, string customerId)
        {
            var customerResult = await GetByIdAsync(id, customerId);
            Result<CustomerProducts> result = Result<CustomerProducts>.Fail("Customer not found");
            if (customerResult.Success)
            {
                CustomerProducts customer = new(customerResult.Data!);
                var query = new QueryDefinition("SELECT * FROM c WHERE c.customerId = @customerId AND c.type = 'product'")
                      .WithParameter("@customerId", customerId);

                List<Product> products = new();
                using (var iterator = _container.GetItemQueryIterator<Product>(query))
                {
                    while (iterator.HasMoreResults)
                    {
                        var response = await iterator.ReadNextAsync();
                        products.AddRange(response);
                    }
                }

                customer.Products = products;
                result= Result<CustomerProducts>.Ok(customer);
            }
            return result;

        }

    }
}
