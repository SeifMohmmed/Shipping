using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class OrderReportControllerTests
{
    private readonly OrderReportController _controller;
    private readonly Mock<IServiceManager> _serviceManager;
    public OrderReportControllerTests()
    {
        _serviceManager = new Mock<IServiceManager>();
        _controller = new OrderReportController(_serviceManager.Object);
    }

    [Fact()]
    public async Task GetAllByPagination_ReturnsOkResult_WithListOfOrderReports()
    {
        //Arrange
        var mockOrderReports = new List<OrderReportToShowDTO>
        {
            new OrderReportToShowDTO {Id=1,MerchantName="Merchant1" },
            new OrderReportToShowDTO {Id=2,MerchantName="Merchant2" }
        };

        _serviceManager.Setup(s => s.orderReportService
        .GetAllOrderReportAsync(It.IsAny<OrderReportPramter>()))
            .ReturnsAsync(mockOrderReports);

        //Act
        var result = await _controller.GetAll(new OrderReportPramter());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedReports = Assert.IsType<List<OrderReportToShowDTO>>(okResult.Value);
        Assert.Equal(2, returnedReports.Count);
    }


    [Fact]
    public async Task GetOrderReport_ReturnsOkResult_WithOrderReport()
    {
        //Arrange

        var mockOrderReport = new OrderReportDTO { Id = 1, ReportDetails = "ReportDetails1" };

        _serviceManager
            .Setup(s => s.orderReportService.GetOrderReportAsync(1))
            .ReturnsAsync(mockOrderReport);


        //Act
        var result = await _controller.GetById(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedOrderReport = Assert.IsType<OrderReportDTO>(okResult.Value);
        Assert.Equal(1, returnedOrderReport.Id);
        Assert.Equal("ReportDetails1", returnedOrderReport.ReportDetails);
    }

    [Fact]
    public async Task GetOrderReport_ThrowsNotFoundException_WhenOrderReportDoesNotExist()
    {
        //Arrange
        _serviceManager.Setup(s => s.orderReportService.GetOrderReportAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(OrderReport), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetById(123));

    }

    [Fact]
    public async Task UpdateOrderReport_ReturnsNoContent_WhenOrderReportIsUpdated()
    {
        // Arrange
        var orderReportToUpdate = new OrderReportDTO { Id = 1, ReportDetails = "ReportDetails1" };
        _serviceManager
            .Setup(s => s.orderReportService.UpdateAsync(1, orderReportToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(1, orderReportToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateOrderReport_ThrowsNotFoundException_WhenOrderReportDoesNotExist()
    {
        //Arrange

        var orderReportToUpdate = new OrderReportDTO { Id = 1, ReportDetails = "ReportDetails1" };

        _serviceManager.Setup(s => s.orderReportService.UpdateAsync(1, orderReportToUpdate))
            .ThrowsAsync(new NotFoundException(nameof(OrderReport), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.Update(1, orderReportToUpdate));

    }

    [Fact]
    public async Task DeleteOrder_ReturnsNoContent_WhenOrderIsDeleted()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.orderReportService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteOrder_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.orderReportService.DeleteAsync(1))
            .ThrowsAsync(new NotFoundException(nameof(OrderReport), "1"));

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.Delete(1));

    }
}