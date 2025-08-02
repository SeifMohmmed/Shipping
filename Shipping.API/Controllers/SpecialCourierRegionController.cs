using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SpecialCourierRegionController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewRegions)]
    public async Task<ActionResult<IEnumerable<SpecialCourierRegionDTO>>> GetAll([FromQuery] PaginationParameters paramter)
    {
        var specialCourierRegion = await serviceManager.specialCourierRegionService.GetSpecialCourierRegionsAsync(paramter);

        return Ok(specialCourierRegion);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewRegions)]
    public async Task<ActionResult<SpecialCourierRegionDTO>> GetById(int id)
    {
        var specialCourierRegion = await serviceManager.specialCourierRegionService.GetSpecialCourierRegionAsync(id);

        if (specialCourierRegion is null)
            return NotFound($"SpecialCourierRegion with id {id} was not found.");

        return Ok(specialCourierRegion);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HasPermission(Permissions.AddRegions)]
    public async Task<ActionResult<SpecialCourierRegionDTO>> Add(SpecialCourierRegionDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid SpecialCourierRegion data");

        await serviceManager.specialCourierRegionService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = DTO.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.UpdateRegions)]
    public async Task<ActionResult<SpecialCourierRegionDTO>> Update(int id, SpecialCourierRegionDTO DTO)
    {
        if (DTO is null || id != DTO.Id)
            return BadRequest("Invalid SpecialCourierRegion data");

        try
        {
            await serviceManager.specialCourierRegionService.UpdateAsync(DTO);

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
    [HasPermission(Permissions.DeleteRegions)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await serviceManager.specialCourierRegionService.DeleteAsync(id);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }
}
