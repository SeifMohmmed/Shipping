using Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.SpecialCourierRegion.Serivce;
public interface ISpecialCourierRegionService
{
    Task<IEnumerable<SpecialCourierRegionDTO>> GetSpecialCourierRegionsAsync(PaginationParameters pramter);
    Task<SpecialCourierRegionDTO> GetSpecialCourierRegionAsync(int id);
    Task AddAsync(SpecialCourierRegionDTO DTO);
    Task UpdateAsync(SpecialCourierRegionDTO DTO);
    Task DeleteAsync(int id);
}
