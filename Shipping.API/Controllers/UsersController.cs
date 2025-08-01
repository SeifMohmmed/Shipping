using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction.User;
using Shipping.Application.Abstraction.User.DTO;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{

    [HttpPost("add-employee")]
    public async Task<IActionResult> AddEmployee(AddEmployeeDTO DTO, CancellationToken cancellationToken)
    {
        var response =
            await userService.AddEmployeeAsync(DTO, cancellationToken);

        if (!string.IsNullOrEmpty(response))
            return BadRequest(response);

        return Created();
    }

    [HttpPost("add-merchant")]
    public async Task<IActionResult> AddMerchant(AddMerchantDTO addMerchantRequest, CancellationToken cancellationToken)
    {
        var response =
            await userService.AddMerchantAsync(addMerchantRequest, cancellationToken);

        if (!string.IsNullOrEmpty(response))
            return BadRequest(response);

        return Created();
    }

    [HttpPost("add-courier")]
    public async Task<IActionResult> AddCourier([FromBody] AddCourierDTO addCourierRequest, CancellationToken cancellationToken)
    {
        var response =
            await userService.AddCourierAsync(addCourierRequest, cancellationToken);

        if (!string.IsNullOrEmpty(response))
            return BadRequest(response);

        return Created();
    }

}
