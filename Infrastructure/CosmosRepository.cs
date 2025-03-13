namespace Infrastructure
{
    using Microsoft.Azure.Cosmos;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class CosmosRepository<T> : IRepository<T> where T : class
    {
        private readonly Container _container;

        public CosmosRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<T?> GetByIdAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string partitionKey)
        {
            var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c WHERE c.partitionKey = @pk")
                .WithParameter("@pk", partitionKey));

            List<T> results = new();
            while (query.HasMoreResults)
            {
                FeedResponse<T> response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task AddAsync(T entity)
        {
            await _container.CreateItemAsync(entity, new PartitionKey(entity!.GetType().GetProperty("PartitionKey")!.GetValue(entity)!.ToString()!));
        }

        public async Task UpdateAsync(T entity)
        {
            await _container.UpsertItemAsync(entity, new PartitionKey(entity!.GetType().GetProperty("PartitionKey")!.GetValue(entity)!.ToString()!));
        }

        public async Task DeleteAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
        }
    }

}
