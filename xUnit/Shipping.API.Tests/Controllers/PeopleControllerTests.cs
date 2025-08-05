using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class PeopleControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly PeopleController _controller;

    public PeopleControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new PeopleController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsOkResult_WithListOfEmployees()
    {
        // Arrange
        var mockEmployees = new List<EmployeeDTO>
        {
            new EmployeeDTO { FullName = "Employee1", Email = "employee1@example.com" },
            new EmployeeDTO { FullName = "Employee2", Email = "employee2@example.com" }
        };

        _serviceManagerMock
            .Setup(s => s.employeeService.GetEmployeesAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockEmployees);

        // Act
        var result = await _controller.GetAllEmployees(new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedEmployees = Assert.IsType<List<EmployeeDTO>>(okResult.Value);
        Assert.Equal(2, returnedEmployees.Count);
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.employeeService.GetEmployeesAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(new List<EmployeeDTO>());

        // Act
        var result = await _controller.GetAllEmployees(new PaginationParameters());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedEmployees = Assert.IsType<List<EmployeeDTO>>(okResult.Value);
        Assert.Empty(returnedEmployees);
    }

    [Fact]
    public async Task GetAllMerchant_ReturnsOkResult_WithListOfMerchants()
    {
        // Arrange
        var mockMerchants = new List<Shipping.Application.Abstraction.People.DTOs.MerchantDTO>
        {
            new Shipping.Application.Abstraction.People.DTOs.MerchantDTO { Id = "1", Name = "Merchant1" },
            new Application.Abstraction.People.DTOs.MerchantDTO  { Id = "2", Name = "Merchant2" }
        };

        _serviceManagerMock
            .Setup(s => s.merchantService.GetMerchantAsync())
            .ReturnsAsync(mockMerchants);

        // Act
        var result = await _controller.GetAllMerchant();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMerchants = Assert.IsType<List<Shipping.Application.Abstraction.People.DTOs.MerchantDTO>>(okResult.Value);
        Assert.Equal(2, returnedMerchants.Count);
    }

    [Fact]
    public async Task GetAllMerchant_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        _serviceManagerMock
            .Setup(s => s.merchantService.GetMerchantAsync())
            .ReturnsAsync(new List<Shipping.Application.Abstraction.People.DTOs.MerchantDTO>());

        // Act
        var result = await _controller.GetAllMerchant();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMerchants = Assert.IsType<List<Shipping.Application.Abstraction.People.DTOs.MerchantDTO>>(okResult.Value);
        Assert.Empty(returnedMerchants);
    }
}