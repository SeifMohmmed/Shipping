using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class ShippingTypeControllerTests
{
    private readonly ShippingTypeController _controller;
    private readonly Mock<IServiceManager> _serviceManager;

    public ShippingTypeControllerTests()
    {
        _serviceManager = new Mock<IServiceManager>();
        _controller = new ShippingTypeController(_serviceManager.Object);
    }

    [Fact()]
    public async Task GetAllShippingTypes_ReturnsOkResult_WithListOfShippingTypes()
    {
        //Arrange
        var mockShippingTypes = new List<ShippingTypeDTO>
        {
            new ShippingTypeDTO{Id=1,BaseCost=50},
            new ShippingTypeDTO{Id=2,BaseCost=50}
        };

        _serviceManager.Setup(s => s.shippingTypeService
        .GetAllShippingTypeAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockShippingTypes);

        //Act

        var result = await _controller.GetAll(new PaginationParameters());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedShippingTypes = Assert.IsType<List<ShippingTypeDTO>>(okResult.Value);
        Assert.Equal(2, returnedShippingTypes.Count);
    }


    [Fact]
    public async Task GetShippingType_ReturnsOkResult_WithShippingType()
    {
        //Arrange

        var mockShippingType = new ShippingTypeDTO { Id = 1, BaseCost = 50 };

        _serviceManager
            .Setup(s => s.shippingTypeService.GetShippingTypeAsync(1))
            .ReturnsAsync(mockShippingType);


        //Act
        var result = await _controller.Get(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedShippingType = Assert.IsType<ShippingTypeDTO>(okResult.Value);
        Assert.Equal(1, returnedShippingType.Id);
        Assert.Equal(50, returnedShippingType.BaseCost);
    }

    [Fact]
    public async Task GetShippingType_ThrowsNotFoundException_WhenShippingTypeDoesNotExist()
    {
        //Arrange
        _serviceManager.Setup(s => s.shippingTypeService.GetShippingTypeAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(ShippingType), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.Get(123));

    }


    [Fact]
    public async Task UpdateShippingType_ReturnsNoContent_WhenShippingTypeIsUpdated()
    {
        // Arrange
        var shippingTypeToUpdate = new ShippingTypeUpdateDTO { Name = "DHL", BaseCost = 50 };

        _serviceManager
            .Setup(s => s.shippingTypeService.UpdateAsync(1, shippingTypeToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(1, shippingTypeToUpdate);

        // Assert
        Assert.IsType<ActionResult<ShippingTypeDTO>>(result);
    }

    [Fact]
    public async Task UpdateShippingType_ThrowsNotFoundException_WhenShippingTypeDoesNotExist()
    {
        //Arrange

        var shippingTypeToUpdate = new ShippingTypeUpdateDTO { Name = "DHL", BaseCost = 50 };

        _serviceManager.Setup(s => s.shippingTypeService.UpdateAsync(1, shippingTypeToUpdate))
            .ThrowsAsync(new NotFoundException(nameof(ShippingType), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.Update(1, shippingTypeToUpdate));

    }

    [Fact]
    public async Task DeleteShippingType_ReturnsNoContent_WhenShippingTypeIsDeleted()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.shippingTypeService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteShippingType_ThrowsNotFoundException_WhenShippingTypeDoesNotExist()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.shippingTypeService.DeleteAsync(1))
            .ThrowsAsync(new NotFoundException(nameof(ShippingType), "1"));

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.Delete(1));

    }
}