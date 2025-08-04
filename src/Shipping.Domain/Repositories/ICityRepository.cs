using Shipping.Domain.Entities;

namespace Shipping.Domain.Repositories;
public interface ICityRepository : IGenericRepository<CitySetting, int>
{
    // This Method Is Used To Get All Cities By Region Id
    Task<IEnumerable<CitySetting>> GetCityByGovernorateName(int regionId);
}
