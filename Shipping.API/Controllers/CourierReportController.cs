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
    public async Task<ActionResult<GetAllCourierOrderCountDTO>> GetBranch(int id)
    {
        var CourierReport = await _serviceManager.courierReportService.GetCourierReportAsync(id);

        return Ok(CourierReport);
    }

}
