using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Region.Service;
public interface IRegionService
{
    Task<IEnumerable<RegionDTO>> GetRegionsAsync(PaginationParameters pramter);
    Task<RegionDTO> GetRegionAsync(int id);
    Task AddAsync(RegionToAddDTO DTO);
    Task UpdateAsync(RegionDTO DTO);
    Task DeleteAsync(int id);
}
