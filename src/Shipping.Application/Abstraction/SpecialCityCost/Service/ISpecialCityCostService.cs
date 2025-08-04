using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.SpecialCityCost.Service;
public interface ISpecialCityCostService
{
    Task<IEnumerable<SpecialCityCostDTO>> GetAllSpecialCityCostAsync(PaginationParameters pramter);
    Task<SpecialCityCostDTO> GetSpecialCityCostAsync(int id);
    Task<SpecialCityAddDTO> AddAsync(SpecialCityAddDTO DTO);
    Task UpdateAsync(int id, SpecialCityUpdateDTO DTO);
    Task DeleteAsync(int id);
}
