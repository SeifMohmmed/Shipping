using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllEmployees([FromQuery] PaginationParameters pramter)
    {
        var employees = await serviceManager.employeeService.GetEmployeesAsync(pramter);

        return Ok(employees);
    }
}
