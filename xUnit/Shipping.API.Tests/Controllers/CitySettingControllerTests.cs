using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Application.Abstraction.CitySettings.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class CitySettingControllerTests
{
    private readonly CitySettingController _citySettingController;
    private readonly Mock<IServiceManager> _serviceManager;
    public CitySettingControllerTests()
    {
        _serviceManager = new Mock<IServiceManager>();
        _citySettingController = new CitySettingController(_serviceManager.Object);
    }

    // TestMethod_Scenario_ExpectResult
    [Fact()]
    public async Task GetCitySettings_ReturnsOkResult_WithListOfCitySettings()
    {
        //Arrange
        var mockCities = new List<CitySettingDTO>
        {
            new CitySettingDTO{Id=1,Name="City1",StandardShippingCost=30},
            new CitySettingDTO{Id=2,Name="City2",StandardShippingCost=50},
        };

        _serviceManager.Setup(s => s.citySettingService
        .GetAllCitySettingAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockCities);

        //Act
        var result = await _citySettingController.GetAll(new PaginationParameters());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCities = Assert.IsType<List<CitySettingDTO>>(okResult.Value);

        Assert.Equal(2, returnedCities.Count);
    }

    [Fact]
    public async Task GetCitySettings_ReturnsOkResult_WithEmptyList()
    {
        _serviceManager.Setup(s => s.citySettingService
        .GetAllCitySettingAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(new List<CitySettingDTO>());

        //Act
        var result = await _citySettingController.GetAll(new PaginationParameters());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCities = Assert.IsType<List<CitySettingDTO>>(okResult.Value);

        Assert.Empty(returnedCities);
    }

    [Fact]
    public async Task GetCitySetting_ReturnsOkResult_WithCitySetting()
    {
        //Arrange
        var mockCity = new CitySettingDTO() { Id = 1, Name = "City1", StandardShippingCost = 30 };

        _serviceManager.Setup(s => s.citySettingService.GetCitySettingAsync(1))
            .ReturnsAsync(mockCity);

        //Act
        var result = await _citySettingController.GetById(1);
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCities = Assert.IsType<CitySettingDTO>(okResult.Value);
        Assert.Equal(1, returnedCities.Id);
        Assert.Equal("City1", returnedCities.Name);
        Assert.Equal(30, returnedCities.StandardShippingCost);
    }

    [Fact]
    public async Task GetCitySetting_ThrowsNotFoundException_WhenCitySettingDoesNotExist()
    {
        //Arrange

        _serviceManager.Setup(s => s.citySettingService.GetCitySettingAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(CitySetting), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _citySettingController.GetById(123));
    }

    [Fact]
    public async Task GetCityByRegionId_ReturnsOkResult_WithListOfCitySettings()
    {
        //Arrange

        var mockCities = new List<CitySettingDTO>()
        {
            new CitySettingDTO{Id=1,Name="City1",StandardShippingCost=30}
        };

        _serviceManager.Setup(s => s.citySettingService.GetCitiesByRegionId(1))
            .ReturnsAsync(mockCities);

        //Act
        var result = await _citySettingController.GetCityByRegion(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCities = Assert.IsType<List<CitySettingDTO>>(okResult.Value);
        Assert.Single(returnedCities);
    }

    [Fact]
    public async Task GetCityByRegionId_ThrowsNotFoundException_WhenCitySettingDoesNotExist()
    {
        //Arrange

        _serviceManager.Setup(s => s.citySettingService.GetCitiesByRegionId(123))
            .ThrowsAsync(new NotFoundException(nameof(CitySetting), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _citySettingController.GetCityByRegion(123));
    }

    [Fact]
    public async Task AddCitySetting_ReturnsActionResult_WithCitySettingToAddDTO()
    {
        // Arrange
        var cityToAdd = new CitySettingToAddDTO { Name = "NewCity", StandardShippingCost = 15 };

        var addedCity = new CitySettingDTO { Id = 1, Name = "NewCity", StandardShippingCost = 15 };

        var mockCitySettingService = new Mock<ICitySettingService>();

        mockCitySettingService
            .Setup(s => s.AddAsync(It.IsAny<CitySettingToAddDTO>()))
            .ReturnsAsync(addedCity);

        _serviceManager
            .Setup(s => s.citySettingService)
            .Returns(mockCitySettingService.Object);

        // Act
        var result = await _citySettingController.Add(cityToAdd);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CitySettingToAddDTO>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnedDTO = Assert.IsType<CitySettingToAddDTO>(createdResult.Value);

        Assert.Equal(cityToAdd.Name, returnedDTO.Name);
        Assert.Equal(cityToAdd.StandardShippingCost, returnedDTO.StandardShippingCost);
    }

    [Fact]
    public async Task UpdateCitySetting_ReturnsNoContent_WhenCitySettingIsUpdated()
    {
        //Arrange
        var mockCitySetting = new CitySettingToUpdateDTO { Name = "UpdatedCity", StandardShippingCost = 25 };

        _serviceManager.Setup(s => s.citySettingService.UpdateAsync(1, mockCitySetting))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _citySettingController.Update(1, mockCitySetting);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCitySetting_ThrowsNotFoundException_WhenCitySettingDoesNotExist()
    {
        //Arrange
        var mockCitySetting = new CitySettingToUpdateDTO { Name = "UpdatedCity", StandardShippingCost = 25 };

        _serviceManager.Setup(s => s.citySettingService.UpdateAsync(1, mockCitySetting))
            .ThrowsAsync(new NotFoundException(nameof(CitySetting), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _citySettingController.Update(1, mockCitySetting));
    }

    [Fact]
    public async Task DeleteCitySetting_ReturnsNoContent_WhenCitySettingIsDelete()
    {
        //Arrange

        _serviceManager.Setup(s => s.citySettingService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _citySettingController.Delete(1);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCitySetting_ThrowsNotFoundException_WhenCitySettingDoesNotExist()
    {
        //Arrange

        _serviceManager.Setup(s => s.citySettingService.DeleteAsync(1))
            .Throws(new NotFoundException(nameof(CitySetting), "1"));

        //Assert

        await Assert.ThrowsAsync<NotFoundException>(() => _citySettingController.Delete(1));

    }
}