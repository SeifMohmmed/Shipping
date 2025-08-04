using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CitySettingController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewCities)]
    public async Task<ActionResult<IEnumerable<CitySettingDTO>>> GetAll([FromQuery] PaginationParameters pram)
    {
        var citySettings = await serviceManager.citySettingService.GetAllCitySettingAsync(pram);

        return Ok(citySettings);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewCities)]
    public async Task<ActionResult<CitySettingDTO>> GetById(int id)
    {
        var citySettings = await serviceManager.citySettingService.GetCitySettingAsync(id);

        return Ok(citySettings);
    }

    [HttpGet("Region/{regionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewCities)]
    public async Task<ActionResult<IEnumerable<CitySettingDTO>>> GetCityByRegion(int regionId)
    {
        var result = await serviceManager.citySettingService.GetCitiesByRegionId(regionId);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HasPermission(Permissions.AddCities)]
    public async Task<ActionResult<CitySettingToAddDTO>> Add(CitySettingToAddDTO DTO)
    {
        var createdCitySetting = await serviceManager.citySettingService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = createdCitySetting.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.UpdateCities)]
    public async Task<IActionResult> Update(int id, CitySettingToUpdateDTO DTO)
    {
        await serviceManager.citySettingService.UpdateAsync(id, DTO);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteCities)]
    public async Task<ActionResult> Delete(int id)
    {
        await serviceManager.citySettingService.DeleteAsync(id);

        return NoContent();
    }


}
