using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Application.Abstraction.Branch.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers.Tests;

public class BranchControllerTests
{
    private readonly BranchController _branchController;
    private readonly Mock<IServiceManager> _serviceManger;
    public BranchControllerTests()
    {
        _serviceManger = new Mock<IServiceManager>();
        _branchController = new BranchController(_serviceManger.Object);
    }

    // TestMethod_Scenario_ExpectResult
    [Fact()]
    public async Task GetBranches_WhenReturnsOk_WithListOfBranches()
    {
        //Arrange

        var mockBranches = new List<BranchDTO>
        {
            new BranchDTO{Id=1,Name="Branch1",Location="Location1"},
            new BranchDTO{Id=2,Name="Branch2",Location="Location2"},
        };

        _serviceManger.Setup(s => s.branchService
        .GetBranchesAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(mockBranches);

        //Act
        var result = await _branchController.GetAll(new PaginationParameters());


        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBranches = Assert.IsType<List<BranchDTO>>(okResult.Value);
        Assert.Equal(2, returnedBranches.Count);

    }

    [Fact]
    public async Task GetBranches_ReturnsOkResult_WithEmptyListOfBranches()
    {
        //Arrange

        _serviceManger.Setup(s => s.branchService
        .GetBranchesAsync(It.IsAny<PaginationParameters>()))
            .ReturnsAsync(new List<BranchDTO>());

        //Act
        var result = await _branchController.GetAll(new PaginationParameters());

        //Assert

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBranches = Assert.IsType<List<BranchDTO>>(okResult.Value);
        Assert.Empty(returnedBranches);
    }

    [Fact]
    public async Task GetBranch_ReturnsOkResult_WithBranch()
    {
        //Arrange

        var mockBranch = new BranchDTO { Id = 1, Name = "Branch1", Location = "Location1" };

        _serviceManger
            .Setup(s => s.branchService.GetBranchAsync(1))
            .ReturnsAsync(mockBranch);


        //Act
        var result = await _branchController.GetById(1);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBranch = Assert.IsType<BranchDTO>(okResult.Value);
        Assert.Equal(1, returnedBranch.Id);
        Assert.Equal("Branch1", returnedBranch.Name);
        Assert.Equal("Location1", returnedBranch.Location);

    }

    [Fact]
    public async Task GetBranch_ThrowsNotFoundException_WhenBranchDoesNotExist()
    {
        //Arrange
        _serviceManger.Setup(s => s.branchService.GetBranchAsync(123))
            .ThrowsAsync(new NotFoundException(nameof(Branch), "123"));


        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _branchController.GetById(123));

    }

    [Fact]
    public async Task AddBranch_ReturnsActionResult_WithBranchToAddDTO()
    {
        // Arrange
        var branchToAdd = new BranchToAddDTO { Name = "NewBranch", Location = "LSA" };

        var addedBranch = new BranchDTO { Id = 1, Name = "NewBranch", Location = "LSA" };

        var mockBranchService = new Mock<IBranchService>();

        mockBranchService
            .Setup(s => s.AddAsync(It.IsAny<BranchToAddDTO>()))
            .ReturnsAsync(addedBranch);

        _serviceManger
            .Setup(s => s.branchService)
            .Returns(mockBranchService.Object);

        // Act
        var result = await _branchController.Add(branchToAdd);

        // Assert
        var actionResult = Assert.IsType<ActionResult<BranchDTO>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var returnedDTO = Assert.IsType<BranchDTO>(createdResult.Value);

        Assert.Equal(branchToAdd.Name, returnedDTO.Name);
        Assert.Equal(branchToAdd.Location, returnedDTO.Location);
    }

    [Fact]
    public async Task UpdateBranch_ReturnsNoContent_WhenBranchIsUpdated()
    {
        //Arrange
        var mockBranch = new BranchToUpdateDTO { Name = "BranchUpdate", Location = "New Location" };

        _serviceManger.Setup(s => s.branchService.UpdateAsync(1, mockBranch))
            .Returns(Task.CompletedTask);

        //Act

        var result = await _branchController.Update(1, mockBranch);

        //Assert
        Assert.IsType<NoContentResult>(result.Result);
    }


    [Fact]
    public async Task UpdateBranch_ThrowsNotFoundException_WhenBranchDoesNotExist()
    {
        //Arrange
        var mockBranch = new BranchToUpdateDTO { Name = "BranchUpdate", Location = "New Location" };

        _serviceManger.Setup(s => s.branchService.UpdateAsync(1, mockBranch))
            .ThrowsAsync(new NotFoundException(nameof(Branch), "1"));

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _branchController.Update(1, mockBranch));

    }

    [Fact]
    public async Task DeleteBranch_ReturnsNoContent_WhenBranchIsDeleted()
    {
        //Arrange
        _serviceManger.Setup(s => s.branchService.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _branchController.Delete(1);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBranch_ThrowsNotFoundException_WhenBranchDoesNotExist()
    {
        //Arrange

        _serviceManger.Setup(s => s.branchService.DeleteAsync(1))
            .Throws(new NotFoundException(nameof(Branch), "1"));

        //Assert

        await Assert.ThrowsAsync<NotFoundException>(() => _branchController.Delete(1));

    }

}