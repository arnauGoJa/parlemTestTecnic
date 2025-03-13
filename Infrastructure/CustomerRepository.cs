using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure
{
    public class CustomerRepository : CosmosRepository<Customer>
    {
        public CustomerRepository(CosmosClient cosmosClient, string databaseName, string containerName)
            : base(cosmosClient, databaseName, containerName) { }
    }

  

}
