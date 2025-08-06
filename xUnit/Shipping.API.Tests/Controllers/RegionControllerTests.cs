using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class RegionControllerTests
{
    private readonly RegionController _regionController;
    private Mock<IServiceManager> _serviceManager;
    public RegionControllerTests()
    {
        _serviceManager = new Mock<IServiceManager>();
        _regionController = new RegionController(_serviceManager.Object);
    }

    [Fact()]
    public async Task GetAllRegion_ReturnsOkResult_WithListOfRegions()
    {
        //Arrange
        var mockRegion = new List<RegionDTO>
        {
            new RegionDTO{Id=1,Governorate="Region1"},
            new RegionDTO{Id=2,Governorate="Region2"},
        };

        _serviceManager.Setup(s => s.regionService
        .GetRegionsAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockRegion);

        //Act

        var result = await _regionController.GetAllRegion(new PaginationParameters());

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRegions = Assert.IsType<List<RegionDTO>>(okResult.Value);
        Assert.Equal(2, returnedRegions.Count);
    }


    [Fact]
    public async Task GetRegion_ReturnsOkResult_WithRegion()
    {
        //Arrange

        var mockRegion = new RegionDTO { Id = 1, Governorate = "Region1" };


        _serviceManager
            .Setup(s => s.regionService.GetRegionAsync(1))
            .ReturnsAsync(mockRegion);


        //Act
        var result = await _regionController.GetById(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedPrdouct = Assert.IsType<RegionDTO>(okResult.Value);
        Assert.Equal(1, returnedPrdouct.Id);
        Assert.Equal("Region1", returnedPrdouct.Governorate);
    }

    [Fact]
    public async Task GetRegion_ThrowsNotFoundException_WhenRegionDoesNotExist()
    {
        //Arrange
        _serviceManager.Setup(s => s.regionService.GetRegionAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(Region), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _regionController.GetById(123));

    }


    [Fact]
    public async Task UpdateRegion_ReturnsNoContent_WhenRegionIsUpdated()
    {
        // Arrange
        var regionToUpdate = new RegionDTO { Id = 1, Governorate = "Region1" };

        _serviceManager
            .Setup(s => s.regionService.UpdateAsync(1, regionToUpdate))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _regionController.Update(1, regionToUpdate);

        // Assert
        Assert.IsType<ActionResult<RegionDTO>>(result);
    }

    [Fact]
    public async Task UpdateRegion_ThrowsNotFoundException_WhenRegionDoesNotExist()
    {
        //Arrange

        var regionToUpdate = new RegionDTO { Id = 1, Governorate = "Region1" };

        _serviceManager.Setup(s => s.regionService.UpdateAsync(1, regionToUpdate))
            .ThrowsAsync(new NotFoundException(nameof(Region), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _regionController.Update(1, regionToUpdate));

    }

    [Fact]
    public async Task DeleteRegion_ReturnsNoContent_WhenRegionIsDeleted()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.regionService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _regionController.DeletRegion(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteRegion_ThrowsNotFoundException_WhenRegionDoesNotExist()
    {
        // Arrange
        _serviceManager
            .Setup(s => s.regionService.DeleteAsync(1))
            .ThrowsAsync(new NotFoundException(nameof(Region), "1"));

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _regionController.DeletRegion(1));

    }

}