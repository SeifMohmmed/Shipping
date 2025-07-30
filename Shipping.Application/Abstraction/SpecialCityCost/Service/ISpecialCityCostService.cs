using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.SpecialCityCost.Service;
public interface ISpecialCityCostService
{
    Task<IEnumerable<SpecialCityCostDTO>> GetAllSpecialCityCostAsync(PaginationParameters pramter);
    Task<SpecialCityCostDTO> GetSpecialCityCostAsync(int id);
    Task AddAsync(SpecialCityCostDTO DTO);
    Task UpdateAsync(SpecialCityCostDTO DTO);
    Task DeleteAsync(int id);
}
