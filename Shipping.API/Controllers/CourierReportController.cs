using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CourierReportController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllCourierOrderCountDTO>>> GetAllReports([FromQuery] PaginationParameters pramter)
    {
        var CourierReports =
            await _serviceManager.courierReportService.GetAllCourierReportAsync(pramter);

        return Ok(CourierReports);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetAllCourierOrderCountDTO>> GetBranch(int id, [FromQuery] PaginationParameters parameter)
    {
        try
        {
            var CourierReport = await _serviceManager.courierReportService.GetCourierReportAsync(id, parameter);
            return Ok(CourierReport);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}
