using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new ProductsController(_serviceManagerMock.Object);
    }

    [Fact()]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        //Arrange
        var mockProducts = new List<ProductDTO>
        {
            new ProductDTO{Name="Product1",Weight=1.0m,Quantity=10},
            new ProductDTO{Name="Product2",Weight=2.0m,Quantity=20}
        };

        _serviceManagerMock.Setup(s => s.productService
        .GetProductsAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockProducts);

        //Act
        var result = await _controller.GetAll(new PaginationParameters());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<List<ProductDTO>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count);
    }

    [Fact]
    public async Task GetProduct_ReturnsOkResult_WithProduct()
    {
        //Arrange

        var mockProduct = new ProductDTO { Id = 1, Name = "Product1", Weight = 1.0m, Quantity = 10 };

        _serviceManagerMock
            .Setup(s => s.productService.GetProductAsync(1))
            .ReturnsAsync(mockProduct);


        //Act
        var result = await _controller.GetById(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPrdouct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Equal(1, returnedPrdouct.Id);
        Assert.Equal("Product1", returnedPrdouct.Name);
        Assert.Equal(1.0m, returnedPrdouct.Weight);
        Assert.Equal(10, returnedPrdouct.Quantity);
    }

    [Fact]
    public async Task GetProduct_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        //Arrange
        _serviceManagerMock.Setup(s => s.productService.GetProductAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(Product), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetById(123));

    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent_WhenProductIsUpdated()
    {
        // Arrange
        var productToUpdate = new UpdateProductDTO { Name = "Product1", Weight = 1.0m, Quantity = 10 };

        _serviceManagerMock
            .Setup(s => s.productService.UpdateAsync(1, productToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(1, productToUpdate);

        // Assert
        Assert.IsType<ActionResult<ProductDTO>>(result);
    }

    [Fact]
    public async Task UpdateProduct_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        //Arrange

        var productToUpdate = new UpdateProductDTO { Name = "Product1", Weight = 1.0m, Quantity = 10 };

        _serviceManagerMock.Setup(s => s.productService.UpdateAsync(1, productToUpdate))
            .ThrowsAsync(new NotFoundException(nameof(Product), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.Update(1, productToUpdate));

    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenProductIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.productService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteOrder_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.productService.DeleteAsync(1))
            .ThrowsAsync(new NotFoundException(nameof(Product), "1"));

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteProduct(1));

    }
}