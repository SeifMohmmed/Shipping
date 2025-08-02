using Microsoft.AspNetCore.Identity;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;
using System.Collections.Concurrent;

namespace Shipping.Infrastructure.Repositories;
internal class UnitOfWork(ApplicationDbContext _context,
    UserManager<ApplicationUser> userManager) : IUnitOfWork
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

    // This Method Is Used To Get The Weight Setting Repository
    public IWeightSettingRepository GetWeightSettingRepository()
    {
        // Check If The Repository Already Exists In The Dictionary Or Not

        return (IWeightSettingRepository)_repositories.GetOrAdd(typeof(WeightSetting).Name, new WeightSettingRepository(_context));
    }

    // This Method Is Used To Get The Special City Cost Repository
    public ISpecialCityCostRepository GetSpecialCityCostRepository()
    {
        return (ISpecialCityCostRepository)_repositories.GetOrAdd(typeof(SpecialCityCost).Name, new SpecialCityCostRepository(_context));
    }

    // This Method Is Used To Get The Order Repository
    public IOrderRepository GetOrderRepository()
    {
        return (IOrderRepository)_repositories.GetOrAdd(typeof(Order).Name, new OrderRepository(_context));
    }

    // This Method Is Used To Get The City Repository
    public ICityRepository GetCityRepository()
    {
        return (ICityRepository)_repositories.GetOrAdd(typeof(CitySetting).Name, new CityRepository(_context));
    }

    // This Method Is Used To Get The Order Report Repository
    public IOrderReportRepository GetOrderReportRepository()
    {
        return (IOrderReportRepository)_repositories.GetOrAdd(typeof(OrderReport).Name, new OrderReportRepository(_context));
    }

    // This Method Is Used To Get The Special Courier Region Repository
    public ISpecialCourierRegionRepository GetSpecialCourierRegionRepository()
    {
        return (ISpecialCourierRegionRepository)_repositories.GetOrAdd(typeof(SpecialCourierRegion).Name, new SpecialCourierRegionRepository(_context));
    }

    // This Method Is Used To Get The Employee Repository
    public IEmployeeRepository GetAllEmployeesAsync()
    {
        return (IEmployeeRepository)_repositories.GetOrAdd(typeof(ApplicationUser).Name, new EmployeeRepository(userManager, _context));
    }

    // This Method Is Used To Get The Merchant Repository
    public IMerchantRepository GetAllMerchantAsync()
    {
        return (IMerchantRepository)_repositories.GetOrAdd(typeof(ApplicationUser).Name, new MerchantRepository(userManager));
    }
}
