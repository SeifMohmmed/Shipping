namespace Shipping.Domain.Repositories;

// This Is A Unit Of Work Interface That Contains The Repositories
public interface IUnitOfWork : IAsyncDisposable
{
    // This Is A Method That Get The Generic Repository And All Repositories
    IGenericRepository<T, Tkey> GetRepository<T, Tkey>()
        where T : class where Tkey : IEquatable<Tkey>;
    IWeightSettingRepository GetWeightSettingRepository();

    ISpecialCityCostRepository GetSpecialCityCostRepository();

    IOrderRepository GetOrderRepository();

    ICityRepository GetCityRepository();

    Task<int> CompleteAsync();

}
