using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Domain.Pramter_Helper;

namespace Shipping.Application.Abstraction.ShippingType.Serivce;
public interface IShippingTypeService
{
    Task<IEnumerable<ShippingTypeDTO>> GetAllShippingTypeAsync(Pramter pramter);
    Task<ShippingTypeDTO> GetShippingTypeAsync(int id);
    Task AddAsync(ShippingTypeDTO DTO);
    Task UpdateAsync(ShippingTypeDTO DTO);
    Task DeleteAsync(int id);
}
