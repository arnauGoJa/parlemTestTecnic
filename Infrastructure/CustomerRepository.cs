using Domain.Entities;
using Domain.Models;
using Microsoft.Azure.Cosmos;

namespace Infrastructure
{
    public class CustomerRepository : CosmosRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CosmosClient cosmosClient, string databaseName, string containerName)
            : base(cosmosClient, databaseName, containerName) { }
    

  
       public async Task<CustomerProducts> GetCustomerWithProductsAsync(string customerId, string partitionKey)
        {
            try
            {
                ItemResponse<Customer> customerResponse = await _container.ReadItemAsync<Customer>(customerId, new PartitionKey(partitionKey));
                CustomerProducts customer = new (customerResponse.Resource);

                var query = new QueryDefinition("SELECT * FROM c WHERE c.partitionKey = @partitionKey AND c.type = 'product'")
                    .WithParameter("@partitionKey", partitionKey);

                List<Product> products = new List<Product>();
                using (FeedIterator<Product> feedIterator = _container.GetItemQueryIterator<Product>(query))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        FeedResponse<Product> response = await feedIterator.ReadNextAsync();
                        products.AddRange(response);
                    }
                }

                customer.Products = products;
                return customer;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
    } 
}
