using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CitySettingController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CitySettingDTO>>> GetAll([FromQuery] PaginationParameters pram)
    {
        var citySettings = await serviceManager.citySettingService.GetAllCitySettingAsync(pram);

        return Ok(citySettings);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CitySettingDTO>> GetById(int id)
    {
        var citySettings = await serviceManager.citySettingService.GetCitySettingAsync(id);

        if (citySettings is null)
            return NotFound($"citySetting with id {id} was not found.");

        return Ok(citySettings);
    }

    [HttpGet("CityByRegion/{regionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<CitySettingDTO>>> GetCityByRegion(int regionId)
    {
        try
        {
            var result = await serviceManager.citySettingService.GetCitiesByRegionId(regionId);

            return Ok(result);
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CitySettingToAddDTO>> Add(CitySettingToAddDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid CitySetting data");

        var createdCitySetting = await serviceManager.citySettingService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = createdCitySetting.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, CitySettingToUpdateDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid CitySetting data");

        try
        {
            await serviceManager.citySettingService.UpdateAsync(id, DTO);

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
            await serviceManager.citySettingService.DeleteAsync(id);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


}
