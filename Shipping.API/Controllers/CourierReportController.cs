using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourierReportController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewCouriers)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetAllCourierOrderCountDTO>>> GetAllReports([FromQuery] PaginationParameters pramter)
    {
        var CourierReports =
            await serviceManager.courierReportService.GetAllCourierReportAsync(pramter);

        return Ok(CourierReports);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewCouriers)]
    public async Task<ActionResult<GetAllCourierOrderCountDTO>> GetBranch(int id, [FromQuery] PaginationParameters parameter)
    {
        var CourierReport = await serviceManager.courierReportService.GetCourierReportAsync(id, parameter);
        return Ok(CourierReport);
    }

}
