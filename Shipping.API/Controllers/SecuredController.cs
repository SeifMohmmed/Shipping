using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Domain.Constants;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SecuredController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = UserRole.Admin)]
    public IActionResult Get()
    {
        return Ok("Hello From Secured Controller");
    }
}
