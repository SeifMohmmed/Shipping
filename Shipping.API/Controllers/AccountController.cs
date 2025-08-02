using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction.User;
using System.Security.Claims;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{

    [HttpGet]
    //To retrieve and return the authenticated user’s account profile information
    public async Task<IActionResult> GetAccountProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var accountProfile = await userService.GetAccountProfileAsync(userId!);
        return Ok(accountProfile);
    }
}
