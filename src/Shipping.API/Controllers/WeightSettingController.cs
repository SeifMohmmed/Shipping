using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.WeightSetting.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WeightSettingController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewSettings)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WeightSettingDTO>>> GetAllWeightSetting([FromQuery] PaginationParameters pramter)
    {
        var allWeightSetting = await serviceManager.weightSettingService.GetAllWeightSettingAsync(pramter);
        return Ok(allWeightSetting);
    }


    [HttpGet("{id}")]
    [HasPermission(Permissions.ViewSettings)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WeightSettingDTO>> GetWeightSetting(int id)
    {
        var WeightSetting = await serviceManager.weightSettingService.GetWeightSettingAsync(id);

        return Ok(WeightSetting);
    }


    [HttpPost]
    [HasPermission(Permissions.AddSettings)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<WeightSettingDTO>> AddWeightSetting(WeightSettingDTO DTO)
    {
        var weightSetting = await serviceManager.weightSettingService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetWeightSetting), new { id = weightSetting.Id }, weightSetting);
    }


    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateSettings)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<WeightSettingDTO>> UpdateWeightSetting(int id, [FromBody] WeightSettingDTO DTO)
    {
        await serviceManager.weightSettingService.UpdateAsync(id, DTO);

        return NoContent();
    }


    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteSettings)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteWeightSetting(int id)
    {
        await serviceManager.weightSettingService.DeleteAsync(id);

        return NoContent();
    }
}
