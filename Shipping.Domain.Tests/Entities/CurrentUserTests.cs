using FluentAssertions;
using Shipping.Domain.Constants;
using Shipping.Domain.Helpers;
using Xunit;

namespace Shipping.Domain.Entities.Tests;

public class CurrentUserTests
{
    // TestMethod_Scenario_ExpectResult
    [Theory()]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.Merchant)]
    public void IsInRoleTest_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        //Arrange

        var currentUser = new CurrentUser("1", "test@gmail.com", [UserRole.Admin, UserRole.Merchant]);

        //Act

        var IsInRole = currentUser.IsInRole(roleName);

        //Assert

        IsInRole.Should().BeTrue();
    }

    [Fact()]
    public void IsInRoleTest_WithNoMatchingRole_ShouldReturnFalse()
    {
        //Arrange

        var currentUser = new CurrentUser("1", "test@gmail.com", [UserRole.Admin, UserRole.Merchant]);

        //Act

        var IsInRole = currentUser.IsInRole(UserRole.Courier);

        //Assert

        IsInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRoleTest_WithNoMatchingRoleCase_ShouldReturnFalse()
    {
        //Arrange

        var currentUser = new CurrentUser("1", "test@gmail.com", [DefaultRole.Admin, DefaultRole.Merchant]);

        //Act

        var IsInRole = currentUser.IsInRole(DefaultRole.Courier.ToLower());

        //Assert

        IsInRole.Should().BeFalse();
    }
}