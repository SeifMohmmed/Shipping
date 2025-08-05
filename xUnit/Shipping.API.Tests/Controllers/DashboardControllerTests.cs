using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction.Dashboard;
using Shipping.Application.Abstraction.Dashboard.DTO;

namespace Shipping.API.Controllers.Tests;

public class DashboardControllerTests
{
    private readonly DashboardController _dashboardController;
    private readonly Mock<IDashboardService> _dashboardService;
    public DashboardControllerTests()
    {
        _dashboardService = new Mock<IDashboardService>();
        _dashboardController = new DashboardController(_dashboardService.Object);
    }

    [Fact]
    public async Task GetDashboardData_ReturnsOkResult_WithEmployeeDashboardData()
    {
        // Arrange
        var mockDashboardData = new EmpDashboardDTO { TotalDelivered = 10, TotalPending = 5 };
        _dashboardService
            .Setup(s => s.GetDashboardOfEmployeeAsync())
            .ReturnsAsync(mockDashboardData);

        // Act
        var result = await _dashboardController.GetDashboardData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<EmpDashboardDTO>(okResult.Value);
        Assert.Equal(10, returnedData.TotalDelivered);
        Assert.Equal(5, returnedData.TotalPending);
    }

    [Fact]
    public async Task GetMerchantDashboardData_ReturnsOkResult_WithMerchantDashboardData()
    {
        // Arrange
        var mockDashboardData = new MerchantDashboardDTO { TotalDelivered = 15, TotalPending = 3 };
        _dashboardService
            .Setup(s => s.GetDashboardDataForMerchantAsync())
            .ReturnsAsync(mockDashboardData);

        // Act
        var result = await _dashboardController.GetMerchantDashboardData();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedData = Assert.IsType<MerchantDashboardDTO>(okResult.Value);
        Assert.Equal(15, returnedData.TotalDelivered);
        Assert.Equal(3, returnedData.TotalPending);
    }
}