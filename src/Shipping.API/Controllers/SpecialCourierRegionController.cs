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
        var specialCourierRegion =
            await serviceManager.specialCourierRegionService.GetSpecialCourierRegionsAsync(paramter);

        return Ok(specialCourierRegion);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewRegions)]
    public async Task<ActionResult<SpecialCourierRegionDTO>> GetById(int id)
    {
        var specialCourierRegion =
            await serviceManager.specialCourierRegionService.GetSpecialCourierRegionAsync(id);

        return Ok(specialCourierRegion);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HasPermission(Permissions.AddRegions)]
    public async Task<ActionResult<SpecialCourierRegionDTO>> Add(SpecialCourierRegionDTO DTO)
    {
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
        await serviceManager.specialCourierRegionService.UpdateAsync(DTO);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteRegions)]
    public async Task<ActionResult> Delete(int id)
    {
        await serviceManager.specialCourierRegionService.DeleteAsync(id);

        return NoContent();
    }
}
