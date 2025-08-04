using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.ShippingType.Serivce;
public interface IShippingTypeService
{
    Task<IEnumerable<ShippingTypeDTO>> GetAllShippingTypeAsync(PaginationParameters pramter);
    Task<ShippingTypeDTO> GetShippingTypeAsync(int id);
    Task AddAsync(ShippingTypeAddDTO DTO);
    Task UpdateAsync(int id, ShippingTypeUpdateDTO DTO);
    Task DeleteAsync(int id);
}
