using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class CourierReportControllerTests
{
    private readonly CourierReportController _controller;
    private readonly Mock<IServiceManager> _serviceManager;
    public CourierReportControllerTests()
    {
        _serviceManager = new Mock<IServiceManager>();
        _controller = new CourierReportController(_serviceManager.Object);
    }

    [Fact]
    public async Task GetAllReports_ReturnsOkResult_WithListOfReports()
    {
        // Arrange
        var mockReports = new List<GetAllCourierOrderCountDTO>
        {
            new GetAllCourierOrderCountDTO { CourierName = "Courier1", OrdersCount = 5 },
            new GetAllCourierOrderCountDTO { CourierName = "Courier2", OrdersCount = 10 }
        };
        _serviceManager
            .Setup(s => s.courierReportService.GetAllCourierReportAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockReports);

        // Act
        var result = await _controller.GetAllReports(new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReports = Assert.IsType<List<GetAllCourierOrderCountDTO>>(okResult.Value);
        Assert.Equal(2, returnedReports.Count);
    }

    [Fact]
    public async Task GetAllReports_ReturnsEmptyList_WhenNoReportsExist()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.courierReportService.GetAllCourierReportAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(new List<GetAllCourierOrderCountDTO>());

        // Act
        var result = await _controller.GetAllReports(new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReports = Assert.IsType<List<GetAllCourierOrderCountDTO>>(okResult.Value);
        Assert.Empty(returnedReports);
    }
}