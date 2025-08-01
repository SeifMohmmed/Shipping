using Shipping.Domain.Entities;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
public class SpecialCourierRegionRepository(ApplicationDbContext context) : ISpecialCourierRegionRepository
{
    public async Task AddRangeAsync(IEnumerable<SpecialCourierRegion> entities)
    {
        await context.SpecialCourierRegions.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
}
