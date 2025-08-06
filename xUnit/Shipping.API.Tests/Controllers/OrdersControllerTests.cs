using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class OrdersControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new OrdersController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllOrdersByStatus_ReturnsOkResult_WithListOfOrders()
    {
        // Arrange
        var mockOrders = new List<OrderWithProductsDTO>
        {
            new OrderWithProductsDTO { Id = 1, Status = "Pending" },
            new OrderWithProductsDTO { Id = 2, Status = "Delivered" }
        };

        _serviceManagerMock
            .Setup(s => s.orderService.GetOrdersByStatus(OrderStatus.Pending, It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockOrders);

        // Act
        var result = await _controller.GetAllOrderByStatus(OrderStatus.Pending, new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedOrders = Assert.IsType<List<OrderWithProductsDTO>>(okResult.Value);
        Assert.Equal(2, returnedOrders.Count);
    }

    [Fact]
    public async Task GetAllOrdersByStatus_ReturnsNotFound_WhenNoOrdersFound()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.GetOrdersByStatus(OrderStatus.Pending, It.IsAny<PaginationParameters>()))
            .ReturnsAsync(new List<OrderWithProductsDTO>());

        // Act
        var result = await _controller.GetAllOrderByStatus(OrderStatus.Pending, new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var orders = Assert.IsType<List<OrderWithProductsDTO>>(okResult.Value);
        Assert.Empty(orders);
    }


    [Fact]
    public async Task GetOrder_ReturnsOkResult_WithOrder()
    {
        //Arrange

        var mockOrder = new OrderWithProductsDTO { Id = 1, Status = "Pending", CustomerName = "Ahmed" };

        _serviceManagerMock
            .Setup(s => s.orderService.GetOrderAsync(1))
            .ReturnsAsync(mockOrder);


        //Act
        var result = await _controller.GetById(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedOrder = Assert.IsType<OrderWithProductsDTO>(okResult.Value);
        Assert.Equal(1, returnedOrder.Id);
        Assert.Equal("Pending", returnedOrder.Status);
        Assert.Equal("Ahmed", returnedOrder.CustomerName);
    }

    [Fact]
    public async Task GetOrder_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        //Arrange
        _serviceManagerMock.Setup(s => s.orderService.GetOrderAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(Order), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetById(123));

    }

    [Fact]
    public async Task AddOrder_ReturnsActionResult_WithBadRequestResult_WhenOrderIsNull()
    {
        // Act
        var result = await _controller.AddOrder(null);

        // Assert
        var actionResult = Assert.IsType<ActionResult<AddOrderDTO>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task AddOrder_ReturnsActionResult_WithOkResult()
    {
        // Arrange
        var orderToAdd = new AddOrderDTO { OrderTypes = OrderType.Delivery, OrderCost = 100 };
        var expectedOrder = new OrderWithProductsDTO { Id = 1, Status = "Pending" };

        _serviceManagerMock
            .Setup(s => s.orderService.AddAsync(orderToAdd))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _controller.AddOrder(orderToAdd);

        // Assert
        var actionResult = Assert.IsType<ActionResult<AddOrderDTO>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(expectedOrder, createdResult.Value);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsNoContent_WhenOrderIsUpdated()
    {
        // Arrange
        var orderToUpdate = new UpdateOrderDTO { Id = 1, OrderTypes = OrderType.Delivery, OrderCost = 150 };
        _serviceManagerMock
            .Setup(s => s.orderService.UpdateAsync(1, orderToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateOrder(1, orderToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateOrder_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        //Arrange

        var orderToUpdate = new UpdateOrderDTO { OrderTypes = OrderType.Delivery, OrderCost = 150 };

        _serviceManagerMock.Setup(s => s.orderService.UpdateAsync(1, orderToUpdate))
            .ThrowsAsync(new NotFoundException(nameof(Order), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateOrder(1, orderToUpdate));

    }

    [Fact]
    public async Task DeleteOrder_ReturnsNoContent_WhenOrderIsDeleted()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteOrder(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteOrder_ThrowsNotFoundException_WhenOrderDoesNotExist()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.DeleteAsync(1))
            .ThrowsAsync(new NotFoundException(nameof(Order), "1"));

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteOrder(1));

    }

    [Fact]
    public async Task AssignOrderToCourier_ReturnsNoContent_WhenOrderIsAssigned()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.orderService.AssignOrderToCourier(1, "courier123"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AssignOrderToCourier(1, "courier123");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AssignOrderToCourier_ThrowsArgumentException_WhenCourierIdIsMissing()
    {
        //Arrange
        _serviceManagerMock
         .Setup(s => s.orderService.AssignOrderToCourier(1, ""))
         .ThrowsAsync(new ArgumentException("CourierId cannot be null or empty."));

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _controller.AssignOrderToCourier(1, ""));

    }


}