using AutoMapper;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Application.Abstraction.Product.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.ProductServices;
public class ProductService(IUnitOfWork _unitOfWork,
    IMapper _mapper) : IProductService
{
    //Get All Products
    public async Task<IEnumerable<ProductDTO>> GetProductsAsync(PaginationParameters pramter)
    => _mapper.Map<IEnumerable<ProductDTO>>(await _unitOfWork.GetRepository<Product, int>().GetAllAsync(pramter));

    // Get Product by Id 
    public async Task<ProductDTO> GetProductAsync(int id)
    => _mapper.Map<ProductDTO>(await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id));


    // Add Product  
    public async Task<ProductDTO> AddAsync(ProductDTO DTO)
    {
        var product = _mapper.Map<Product>(DTO);

        await _unitOfWork.GetRepository<Product, int>().AddAsync(product);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<ProductDTO>(product);
    }


    //Update Proudct
    public async Task UpdateAsync(int id, UpdateProductDTO DTO)
    {
        var productRepo = _unitOfWork.GetRepository<Product, int>();

        var existingProduct = await productRepo.GetByIdAsync(id);

        if (existingProduct is null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        _mapper.Map(DTO, existingProduct);

        productRepo.UpdateAsync(existingProduct);

        await _unitOfWork.CompleteAsync();
    }

    // Delete Product  
    public async Task DeleteAsync(int id)
    {
        var productRepo = _unitOfWork.GetRepository<Product, int>();

        var existingProduct = await productRepo.GetByIdAsync(id);

        if (existingProduct is null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        await productRepo.DeleteAsync(id);

        await _unitOfWork.CompleteAsync();
    }
}
