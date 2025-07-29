using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RegionController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RegionDTO>>> GetAll([FromQuery] PaginationParameters parameters)
    {
        var regions = await serviceManager.regionService.GetRegionsAsync(parameters);

        return Ok(regions);

    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegionDTO>> GetById(int id)
    {
        var region = await serviceManager.regionService.GetRegionAsync(id);

        if (region is null)
            return NotFound($"Region with id {id} was not found.");

        return Ok(region);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RegionDTO>> Add(RegionToAddDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid Region data");

        await serviceManager.regionService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = DTO.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegionDTO>> Update(int id, RegionDTO DTO)
    {
        if (DTO is null || id != DTO.Id)
            return BadRequest("Invalid Region data");
        try
        {
            await serviceManager.regionService.UpdateAsync(DTO);
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
            await serviceManager.regionService.DeleteAsync(id);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
