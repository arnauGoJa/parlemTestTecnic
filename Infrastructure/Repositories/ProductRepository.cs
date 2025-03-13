using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.Repositories
{
    public class ProductRepository : CosmosRepository<Product>
    {
        public ProductRepository(CosmosClient cosmosClient, string databaseName, string containerName)
            : base(cosmosClient, databaseName, containerName) { }
    }
}
