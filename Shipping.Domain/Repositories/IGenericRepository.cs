using Shipping.Domain.Helpers;

namespace Shipping.Domain.Repositories;

// This Is A Generic Repository Interface That Contains The Basic CRUD Operations
public interface IGenericRepository<T, Tkey> where T : class where Tkey : IEquatable<Tkey>
{
    // This Is CRUD Operations Methods   
    Task<IEnumerable<T>> GetAllAsync(PaginationParameters pramter);
    Task<IEnumerable<T>> GetAllAsync(PaginationParameters pramter, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task<T?> GetByIdAsync(Tkey id);
    Task<T?> GetByIdAsync(Tkey id, Func<IQueryable<T>, IQueryable<T>>? include = null);
    Task AddAsync(T entity);
    void UpdateAsync(T entity);
    Task DeleteAsync(Tkey id);
}
