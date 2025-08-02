using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Courier.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourierController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet("Region")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CourierDTO>>> GetCouriersByRegion([FromQuery] int regionId, [FromQuery] PaginationParameters parameters)
    {
        var couriers =
            await serviceManager.courierService.GetCourierByRegion(regionId, parameters);

        return Ok(couriers);
    }

    [HttpGet("Order")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CourierDTO>>> GetCourierByBranch([FromQuery] int orderId, [FromQuery] PaginationParameters parameters)
    {
        var couriers =
            await serviceManager.courierService.GetCourierByBranch(orderId);

        return Ok(couriers);
    }
}
