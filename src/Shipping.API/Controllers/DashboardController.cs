using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction.Dashboard;
using Shipping.Application.Abstraction.Dashboard.DTO;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DashboardController(IDashboardService serviceManager) : ControllerBase
{
    [HttpGet("employee-dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<EmpDashboardDTO>> GetDashboardData()
    {
        var dashboardData = await serviceManager.GetDashboardOfEmployeeAsync();
        return Ok(dashboardData);
    }

    [HttpGet("merchant-dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MerchantDashboardDTO>> GetMerchantDashboardData()
    {
        var dashboardData = await serviceManager.GetDashboardDataForMerchantAsync();
        return Ok(dashboardData);
    }
}
