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
    public async Task<ActionResult<IEnumerable<WeightSettingDTO>>> GetAllWeightSetting([FromQuery] PaginationParameters pramter)
    {
        var allWeightSetting = await serviceManager.weightSettingService.GetAllWeightSettingAsync(pramter);
        return Ok(allWeightSetting);
    }


    [HttpGet("{id}")]
    [HasPermission(Permissions.ViewSettings)]
    public async Task<ActionResult<WeightSettingDTO>> GetWeightSetting(int id)
    {
        var WeightSetting = await serviceManager.weightSettingService.GetWeightSettingAsync(id);
        return Ok(WeightSetting);
    }


    [HttpPost]
    [HasPermission(Permissions.AddSettings)]
    public async Task<ActionResult<WeightSettingDTO>> AddWeightSetting(WeightSettingDTO DTO)
    {
        await serviceManager.weightSettingService.AddAsync(DTO);
        return Ok();
    }


    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateSettings)]
    public async Task<ActionResult<WeightSettingDTO>> UpdateWeightSetting(int id, [FromBody] WeightSettingDTO DTO)
    {
        try
        {
            await serviceManager.weightSettingService.UpdateAsync(id, DTO);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeleteSettings)]
    public async Task<ActionResult> DeleteWeightSetting(int id)
    {
        try
        {
            await serviceManager.weightSettingService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
