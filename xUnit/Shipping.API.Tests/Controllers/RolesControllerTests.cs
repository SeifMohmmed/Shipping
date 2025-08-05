using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction.Roles;
using Shipping.Application.Abstraction.Roles.DTO;

namespace Shipping.API.Controllers.Tests;

public class RolesControllerTests
{
    private readonly RolesController _rolesController;
    private Mock<IRoleService> _roleService;

    public RolesControllerTests()
    {
        _roleService = new Mock<IRoleService>();
        _rolesController = new RolesController(_roleService.Object);
    }

    [Fact]
    public async Task GetAllRoles_ReturnsOkResult_WithListOfRoles()
    {
        // Arrange
        var mockRoles = new List<RoleResponseDTO>
        {
            new RoleResponseDTO ( RoleId : "1", RoleName : "Group1", CreatedAt : "2023-01-01" ),
            new RoleResponseDTO (RoleId : "2", RoleName : "Group2", CreatedAt : "2023-01-02")
        };

        _roleService
            .Setup(s => s.GetAllRolesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockRoles);

        // Act
        var result = await _rolesController.GetAll(CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGroups = Assert.IsType<List<RoleResponseDTO>>(okResult.Value);
        Assert.Equal(2, returnedGroups.Count);
    }

    [Fact]
    public async Task GetRole_ReturnsOkResult_WithRole()
    {
        // Arrange
        var mockRole = new RoleDetailsResponseDTO(
            RoleId: "1",
            RoleName: "Group1",
            CreatedAt: "2023-01-01",
            Permissions: new List<string>()
        );

        _roleService
            .Setup(s => s.GetRoleByIdAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockRole);

        // Act
        var result = await _rolesController.GetById("1", CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGroup = Assert.IsType<RoleDetailsResponseDTO>(okResult.Value);
        Assert.Equal("1", returnedGroup.RoleId);
    }

    [Fact]
    public async Task GetRole_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        _roleService
            .Setup(s => s.GetRoleByIdAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync((RoleDetailsResponseDTO)null);

        // Act
        var result = await _rolesController.GetById("1", CancellationToken.None);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var message = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Role does not exists", message);

    }

    [Fact]
    public async Task AddRole_ReturnsOkResult_WhenRoleIsCreated()
    {
        // Arrange
        var createRequest = new CreateRoleRequestDTO
        (
            RoleName: "NewRole",
            Permissions: new List<string>()
        );
        _roleService
            .Setup(s => s.CreateRoleAsync(createRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync("Role Added Successfully!");

        // Act
        var result = await _rolesController.Add(createRequest, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<string>(okResult.Value);
        Assert.Equal("Role Added Successfully!", response);
    }

    [Fact]
    public async Task DeleteRole_ReturnsOkResult_WhenRoleIsDeleted()
    {
        // Arrange
        _roleService
            .Setup(s => s.DeleteRoleAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync("Role Deleted Successfully!");

        // Act
        var result = await _rolesController.Delete("1", CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<string>(okResult.Value);
        Assert.Equal("Role Deleted Successfully!", response);
    }
}