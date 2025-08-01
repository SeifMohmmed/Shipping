using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SecuredController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello From Secured Controller");
    }
}
