using Shipping.Domain.Entities;

namespace Shipping.Domain.Repositories;
public interface ISpecialCourierRegionRepository
{
    // This Method Is Used To Add A Range Of SpecialCourierRegion Entities To The Database
    Task AddRangeAsync(IEnumerable<SpecialCourierRegion> entities);
}
