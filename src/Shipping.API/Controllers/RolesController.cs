using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction.Roles;
using Shipping.Application.Abstraction.Roles.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewPermissions)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await roleService.GetAllRolesAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.ViewPermissions)]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await roleService.GetRoleByIdAsync(id, cancellationToken);

        if (result is null)
            return NotFound("Role does not exists");

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(Permissions.AddPermissions)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken)
    {
        var result = await roleService.CreateRoleAsync(createRoleRequestDTO, cancellationToken);

        if (result.Equals("Role Added Successfully!"))
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.UpdatePermissions)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(string id, CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken)
    {
        var result =
            await roleService.UpdateRoleAsync(id, createRoleRequestDTO, cancellationToken);

        if (result.Equals("Role Updated Successfully!"))
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.DeletePermissions)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result =
            await roleService.DeleteRoleAsync(id, cancellationToken);

        if (result.Equals("Role Deleted Successfully!"))
            return Ok(result);

        return BadRequest(result);
    }
}
