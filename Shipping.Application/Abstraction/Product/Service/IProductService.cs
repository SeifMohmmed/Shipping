﻿using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Product.Service;
public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync(PaginationParameters pramter);
    Task<ProductDTO> GetProductAsync(int id);
    Task<ProductDTO> AddAsync(ProductDTO DTO);
    Task UpdateAsync(int id, UpdateProductDTO DTO);
    Task DeleteAsync(int id);
}
