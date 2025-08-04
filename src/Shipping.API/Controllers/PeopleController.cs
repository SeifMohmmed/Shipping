using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Helpers;
namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PeopleController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet("employees")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewEmployees)]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllEmployees([FromQuery] PaginationParameters parameter)
    {
        var employees = await serviceManager.employeeService.GetEmployeesAsync(parameter);

        return Ok(employees);
    }

    [HttpGet("merchants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewMerchants)]
    public async Task<ActionResult<IEnumerable<MerchantDTO>>> GetAllMerchant()
    {
        var merchants = await serviceManager.merchantService.GetMerchantAsync();

        return Ok(merchants);
    }
}
