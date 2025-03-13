namespace Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id, string partitionKey);
        Task<IEnumerable<T>> GetAllAsync(string partitionKey);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id, string partitionKey);
    }
}
