using Shipping.Domain.Helpers;

namespace Shipping.Domain.Repositories;

// This Is A Generic Repository Interface That Contains The Basic CRUD Operations
public interface IGenericRepository<T, Tkey> where T : class where Tkey : IEquatable<Tkey>
{
    // This Is CRUD Operations Methods   
    Task<IEnumerable<T>> GetAllAsync(PaginationParameters pramter);
    Task<T?> GetByIdAsync(Tkey id);
    Task AddAsync(T entity);
    void UpdateAsync(T entity);
    Task DeleteAsync(Tkey id);
}
