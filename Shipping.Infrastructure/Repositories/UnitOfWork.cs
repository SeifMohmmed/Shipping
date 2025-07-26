using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;
using System.Collections.Concurrent;

namespace Shipping.Infrastructure.Repositories;
internal class UnitOfWork(ApplicationDbContext _context) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = new();

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    // This Method Is Used To Get A Generic Repository For A Specific Entity Type
    public IGenericRepository<T, Tkey> GetRepository<T, Tkey>()
        where T : class
        where Tkey : IEquatable<Tkey>
    {
        // Check If The Repository Already Exists In The Dictionary Or Not
        return (IGenericRepository<T, Tkey>)_repositories.GetOrAdd(typeof(T).Name,
            new GenericRepository<T, Tkey>(_context));
    }

}
