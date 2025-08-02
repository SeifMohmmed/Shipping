using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SpecialCityCostController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewCities)]
    public async Task<ActionResult<IEnumerable<SpecialCityCostDTO>>> GetAll([FromQuery] PaginationParameters pram)
    {
        var specialCity = await serviceManager.specialCityCostService.GetAllSpecialCityCostAsync(pram);

        return Ok(specialCity);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewCities)]
    public async Task<ActionResult<SpecialCityCostDTO>> GetById(int id)
    {
        var speicalCity = await serviceManager.specialCityCostService.GetSpecialCityCostAsync(id);

        if (speicalCity is null)
            return NotFound($"SpeicalCity with id {id} was not found.");

        return Ok(speicalCity);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.AddCities)]
    public async Task<ActionResult<SpecialCityAddDTO>> Add(SpecialCityAddDTO DTO)
    {
        await serviceManager.specialCityCostService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = DTO.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateCities)]
    public async Task<ActionResult<SpecialCityCost>> Update(int id, SpecialCityUpdateDTO DTO)
    {
        try
        {
            await serviceManager.specialCityCostService.UpdateAsync(id, DTO);

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
    [HasPermission(Permissions.DeleteCities)]
    public async Task<ActionResult<SpecialCityCost>> Delete(int id)
    {
        try
        {
            await serviceManager.specialCityCostService.DeleteAsync(id);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}
