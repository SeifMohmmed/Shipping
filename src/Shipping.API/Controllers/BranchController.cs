using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BranchController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewBranches)]
    public async Task<ActionResult<IEnumerable<BranchDTO>>> GetAll([FromQuery] PaginationParameters param)
    {
        var branches = await serviceManager.branchService.GetBranchesAsync(param);

        return Ok(branches);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewBranches)]
    public async Task<ActionResult<BranchDTO>> GetById(int id)
    {
        var branch = await serviceManager.branchService.GetBranchAsync(id);

        return Ok(branch);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HasPermission(Permissions.AddBranches)]
    public async Task<ActionResult<BranchDTO>> Add([FromBody] BranchToAddDTO DTO)
    {
        var branch = await serviceManager.branchService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = branch.Id }, branch);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.UpdateBranches)]
    public async Task<ActionResult<BranchDTO>> Update(int id, [FromBody] BranchToUpdateDTO DTO)
    {
        await serviceManager.branchService.UpdateAsync(id, DTO);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteBranches)]
    public async Task<ActionResult> Delete(int id)
    {
        await serviceManager.branchService.DeleteAsync(id);

        return NoContent();
    }
}
