using AutoMapper;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Application.Abstraction.Product.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.ProductServices;
public class ProductService(ILogger<ProductService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IProductService
{
    //Get All Products
    public async Task<IEnumerable<ProductDTO>> GetProductsAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all products with pagination: {@PaginationParameters}", pramter);

        var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(pramter);

        return mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    // Get Product by Id 
    public async Task<ProductDTO> GetProductAsync(int id)
    {
        logger.LogInformation("Retrieving product with ID: {ProductId}", id);

        var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);

        if (product is null)
            throw new NotFoundException(nameof(Product), id.ToString());

        return mapper.Map<ProductDTO>(product);
    }

    // Add Product  
    public async Task<ProductDTO> AddAsync(ProductDTO DTO)
    {
        logger.LogInformation("Adding new product: {@ProductDTO}", DTO);

        var product = mapper.Map<Product>(DTO);

        await unitOfWork.GetRepository<Product, int>().AddAsync(product);

        await unitOfWork.CompleteAsync();

        return mapper.Map<ProductDTO>(product);
    }


    //Update Proudct
    public async Task UpdateAsync(int id, UpdateProductDTO DTO)
    {
        logger.LogInformation("Updating product with ID: {ProductId} using data: {@UpdateProductDTO}", id, DTO);

        var productRepo = unitOfWork.GetRepository<Product, int>();

        var existingProduct = await productRepo.GetByIdAsync(id);

        if (existingProduct is null)
            throw new NotFoundException(nameof(Product), id.ToString());

        mapper.Map(DTO, existingProduct);

        productRepo.UpdateAsync(existingProduct);

        await unitOfWork.CompleteAsync();
    }

    // Delete Product  
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);

        var productRepo = unitOfWork.GetRepository<Product, int>();

        var existingProduct = await productRepo.GetByIdAsync(id);

        if (existingProduct is null)
            throw new NotFoundException(nameof(Product), id.ToString());

        await productRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }
}
