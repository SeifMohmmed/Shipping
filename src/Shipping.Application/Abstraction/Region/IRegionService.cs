using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Region;
public interface IRegionService
{
    Task<IEnumerable<RegionDTO>> GetRegionsAsync(PaginationParameters pramter);
    Task<RegionDTO> GetRegionAsync(int id);
    Task<RegionDTO> AddAsync(RegionDTO DTO);
    Task UpdateAsync(int id, RegionDTO DTO);
    Task DeleteAsync(int id);
}
