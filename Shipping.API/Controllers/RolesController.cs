using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction.Auth;
using Shipping.Application.Abstraction.Roles.DTO;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await roleService.GetAllRolesAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await roleService.GetRoleByIdAsync(id, cancellationToken);

        if (result is null)
            return NotFound("Group does not exists");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken)
    {
        var result = await roleService.CreateRoleAsync(createRoleRequestDTO, cancellationToken);

        if (result.Equals("Group Created Successfully!"))
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(string id, CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken)
    {
        var result =
            await roleService.UpdateRoleAsync(id, createRoleRequestDTO, cancellationToken);

        if (result.Equals("Group Updated Successfully!"))
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result =
            await roleService.DeleteRoleAsync(id, cancellationToken);

        if (result.Equals("Group Deletd Successfully!"))
            return Ok(result);

        return BadRequest(result);
    }
}
