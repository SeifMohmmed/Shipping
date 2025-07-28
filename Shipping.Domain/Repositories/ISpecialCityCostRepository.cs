using Shipping.Domain.Entities;

namespace Shipping.Domain.Repositories;
public interface ISpecialCityCostRepository : IGenericRepository<SpecialCityCost, int>
{
    // This Is A Methods That Add A Range Of SpecialCityCost And Get City Cost By MerchantId
    Task AddRangeAsync(IEnumerable<SpecialCityCost> entities);
    Task<SpecialCityCost> GetCityCostByMarchantId(string MerchantId, int CityId);

}
