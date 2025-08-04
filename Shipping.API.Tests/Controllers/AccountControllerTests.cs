using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shipping.Application.Abstraction.User;
using Shipping.Application.Abstraction.User.DTO;
using System.Security.Claims;

namespace Shipping.API.Controllers.Tests;

public class AccountControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new AccountController(_userServiceMock.Object);

        // Mock the User context for the controller
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "12345") // Mocked user ID
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    // TestMethod_Scenario_ExpectResult
    [Fact()]
    public async Task GetAccountProfile_ReturnsOkResult_WithAccountProfile()
    {
        //Arrange
        var mockProfile = new AccountProfileDTO
        (
            Email: "test@gmail.com",
            FullName: "test",
            PhoneNumber: "1233344",
            Address: "123 st"
        );

        _userServiceMock.Setup(s =>
        s.GetAccountProfileAsync("12345", default))
            .ReturnsAsync(mockProfile);

        //Act
        var result = await _controller.GetAccountProfile();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProfile = Assert.IsType<AccountProfileDTO>(okResult.Value);

        Assert.Equal("test@gmail.com", returnedProfile.Email);

    }

    [Fact()]
    public async Task GetAccountProfile_ReturnsNotFound_WhenProfileIsNull()
    {
        //Arrange

        _userServiceMock.Setup(s =>
        s.GetAccountProfileAsync("12345", default))
            .ReturnsAsync((AccountProfileDTO?)null);

        //Act

        var result = await _controller.GetAccountProfile();

        //Assert

        Assert.IsType<NotFoundResult>(result);

    }

    [Fact]
    public async Task GetAccountProfile_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        //Arrange

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity()) // No authenticated identity
            }
        };

        //Act
        var result = await _controller.GetAccountProfile();

        //Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task GetAccountProfile_ReturnsUnauthorized_WhenUserIdclaimsMissing()
    {
        //Arrange

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, "test@gmail.com") // Different claim, no NameIdentifier
        }, "test"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        //Act
        var result = await _controller.GetAccountProfile();

        //Assert
        Assert.IsType<UnauthorizedResult>(result);

    }
}