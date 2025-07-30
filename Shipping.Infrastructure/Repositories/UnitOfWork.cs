using Shipping.Domain.Entities;
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

    public IWeightSettingRepository GetWeightSettingRepository()
    {
        return (IWeightSettingRepository)_repositories.GetOrAdd(typeof(WeightSetting).Name, new WeightSettingRepository(_context));
    }

    public ISpecialCityCostRepository GetSpecialCityCostRepository()
    {
        return (ISpecialCityCostRepository)_repositories.GetOrAdd(typeof(SpecialCityCost).Name, new SpecialCityCostRepository(_context));
    }

    public IOrderRepository GetOrderRepository()
    {
        return (IOrderRepository)_repositories.GetOrAdd(typeof(Order).Name, new OrderRepository(_context));
    }
    public ICityRepository GetCityRepository()
    {
        return (ICityRepository)_repositories.GetOrAdd(typeof(CitySetting).Name, new CityRepository(_context));
    }
}
