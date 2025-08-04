using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Shipping.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Shipping.Domain.Entities.Tests;

public class UserContextTests
{
    // TestMethod_Scenario_ExpectResult
    [Fact()]
    public void GetCurrentUserTest_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        //AAA

        //Arrange

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var claims = new List<Claim>()
        {
            new Claim (ClaimTypes.NameIdentifier,"1"),
            new Claim (ClaimTypes.Email,"test@gmail.com"),
            new Claim (ClaimTypes.Role, UserRole.Merchant),
            new Claim(ClaimTypes.Role, UserRole.Courier)
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        httpContextAccessorMock.Setup(s => s.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });


        var userContext = new UserContext(httpContextAccessorMock.Object);

        //Act

        var currentUser = userContext.GetCurrentUser();

        //Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@gmail.com");
        currentUser.Roles.Should().ContainInOrder(UserRole.Merchant, UserRole.Courier);
    }


    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // act

        Action action = () => userContext.GetCurrentUser();

        // assert 

        action.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }
}