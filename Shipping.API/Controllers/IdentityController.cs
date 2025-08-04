using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction.User;
using Shipping.Domain.Constants;
using Shipping.Domain.Entities;

namespace Shipping.API.Controllers;
[Route("api/idenity")]
[ApiController]
[Authorize]
public class IdentityController(IUserService userService) : ControllerBase
{
    [HttpPatch("user")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDetails(UpdateUserDetails request, CancellationToken cancellationToken)
    {
        await userService.UpdateUserDetails(request, cancellationToken);

        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRole.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole(AssignUserRoles request)
    {
        await userService.AssignUserRoles(request);

        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRole.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUserRole(UnassignUserRoles request)
    {
        await userService.UnassignUserRoles(request);

        return NoContent();
    }
}
