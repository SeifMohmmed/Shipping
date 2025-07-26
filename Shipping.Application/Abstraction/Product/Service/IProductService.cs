using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Pramter_Helper;

namespace Shipping.Application.Abstraction.Product.Service;
public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync(Pramter pramter);
    Task<ProductDTO> GetProductAsync(int id);
    Task AddAsync(ProductDTO DTO);
    Task UpdateAsync(UpdateProductDTO DTO);
    Task DeleteAsync(int id);
}
