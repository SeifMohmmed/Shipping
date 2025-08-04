using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RegionController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewRegions)]
    public async Task<ActionResult<IEnumerable<RegionDTO>>> GetAllRegion([FromQuery] PaginationParameters pramter)
    {
        var regions = await serviceManager.regionService.GetRegionsAsync(pramter);
        return Ok(regions);
    }


    [HttpGet("{id}")]
    [HasPermission(Permissions.ViewRegions)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegionDTO>> GetById(int id)
    {
        var region = await serviceManager.regionService.GetRegionAsync(id);

        return Ok(region);
    }


    [HttpPost]
    [HasPermission(Permissions.AddRegions)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<RegionDTO>> Add(RegionDTO DTO)
    {
        var region = await serviceManager.regionService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = region.Id }, region);
    }


    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateRegions)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegionDTO>> Update(int id, [FromBody] RegionDTO DTO)
    {
        await serviceManager.regionService.UpdateAsync(id, DTO);

        return NoContent();
    }


    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteRegions)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeletRegion(int id)
    {
        await serviceManager.regionService.DeleteAsync(id);

        return NoContent();
    }

}
