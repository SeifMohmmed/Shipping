using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class CityRepository(ApplicationDbContext context) : GenericRepository<CitySetting, int>(context), ICityRepository
{
    public async Task<IEnumerable<CitySetting>> GetCityByGovernorateName(int regionId)
    {
        return await context.CitySettings
            .Where(c => c.RegionId == regionId)
            .ToListAsync();
    }
}
