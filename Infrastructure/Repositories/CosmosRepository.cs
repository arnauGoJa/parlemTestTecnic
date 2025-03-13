namespace Infrastructure.Repositories
{
    using Domain.Interfaces;
    using Domain.Models;
    using Microsoft.Azure.Cosmos;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    public class CosmosRepository<T> : IRepository<T> where T : class
    {
        protected readonly Container _container;

        public CosmosRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<Result<T>> GetByIdAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return Result<T>.Ok(response.Resource);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Result<T>.Fail("Item not found");
            }
        }

        public async Task<Result<IEnumerable<T>>> GetAllAsync(string partitionKey)
        {
            var query = _container.GetItemQueryIterator<T>(
                new QueryDefinition("SELECT * FROM c WHERE c.partitionKey = @pk")
                    .WithParameter("@pk", partitionKey));

            List<T> results = new List<T>();

            while (query.HasMoreResults)
            {
                FeedResponse<T> response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return Result<IEnumerable<T>>.Ok(results);
        }

        public async Task<Result<bool>> AddAsync(T entity)
        {
            try
            {
                await _container.CreateItemAsync(entity, new PartitionKey(GetPartitionKey(entity)));
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Error adding entity: " + ex.Message);
            }
        }

        public async Task<Result<bool>> UpdateAsync(T entity)
        {
            try
            {
                await _container.UpsertItemAsync(entity, new PartitionKey(GetPartitionKey(entity)));
                return Result<bool>.Ok(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Update failed: " + ex.Message);
            }
        }


        public async Task<Result<bool>> DeleteAsync(string id, string partitionKey)
        {
            try
            {
                await _container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
                return Result<bool>.Ok(true);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return Result<bool>.Fail("Item not found");
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("Error deleting item: " + ex.Message);
            }
        }
        private string GetPartitionKey(T entity)
        {
            return entity.GetType().GetProperty("CustomerId")?.GetValue(entity)?.ToString() ?? string.Empty;
        }

    }
}


