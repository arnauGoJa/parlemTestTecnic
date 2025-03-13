using Domain.Models;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Result<T>> GetByIdAsync(string id, string partitionKey);
        Task<Result<IEnumerable<T>>> GetAllAsync(string partitionKey);
        Task<Result<bool>> AddAsync(T entity);
        Task<Result<bool>> UpdateAsync(T entity);
        Task<Result<bool>> DeleteAsync(string id, string partitionKey);
    }
}
