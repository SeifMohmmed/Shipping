using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BranchController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BranchDTO>>> GetAll([FromQuery] PaginationParameters param)
    {
        var branches = await serviceManager.branchService.GetBranchesAsync(param);

        return Ok(branches);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BranchDTO>> GetById(int id)
    {
        var branch = await serviceManager.branchService.GetBranchAsync(id);

        if (branch is null)
            return NotFound();

        return Ok(branch);
    }

    [HttpPost]
    public async Task<ActionResult<BranchDTO>> Add([FromBody] BranchToAddDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid branch data");

        var branch = await serviceManager.branchService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = branch.Id }, branch);

    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BranchDTO>> Update(int id, [FromBody] BranchToUpdateDTO DTO)
    {
        if (DTO is null || id != DTO.Id)
            return BadRequest("Invalid branch data");
        try
        {
            await serviceManager.branchService.UpdateAsync(DTO);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await serviceManager.branchService.DeleteAsync(id);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
