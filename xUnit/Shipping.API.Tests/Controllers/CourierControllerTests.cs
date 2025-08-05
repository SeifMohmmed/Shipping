using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Courier.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class CourierControllerTests
{
    private readonly CourierController _controller;
    private readonly Mock<IServiceManager> _serviceManager;
    public CourierControllerTests()
    {
        _serviceManager = new Mock<IServiceManager>();
        _controller = new CourierController(_serviceManager.Object);
    }

    [Fact()]
    public async Task GetCouriersByBranch_ReturnsOkResult_WithListOfCouriers()
    {
        //Arrage
        var mockCouriers = new List<CourierDTO>
        {
            new CourierDTO { CourierId = "1", CourierName = "Courier1" },
            new CourierDTO { CourierId = "2", CourierName = "Courier2" }
        };

        _serviceManager.Setup(s => s.courierService.GetCourierByBranch(1))
            .ReturnsAsync(mockCouriers);

        //Act

        var result = await _controller.GetCourierByBranch(1, new PaginationParameters());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCouriers = Assert.IsType<List<CourierDTO>>(okResult.Value);
        Assert.Equal(2, returnedCouriers.Count);

    }

    [Fact]
    public async Task GetCouriersByRegion_ReturnsOkResult_WithListOfCouriers()
    {
        // Arrange
        var mockCouriers = new List<CourierDTO>
        {
            new CourierDTO { CourierId = "1", CourierName = "Courier1" }
        };
        _serviceManager
            .Setup(s => s.courierService.GetCourierByRegion(1, It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockCouriers);

        // Act
        var result = await _controller.GetCouriersByRegion(1, new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCouriers = Assert.IsType<List<CourierDTO>>(okResult.Value);
        Assert.Single(returnedCouriers);
    }
}