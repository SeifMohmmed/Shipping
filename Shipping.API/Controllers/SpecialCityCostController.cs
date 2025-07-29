using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<SpecialCityCostDTO>>> GetAll([FromQuery] PaginationParameters pram)
    {
        var specialCity = await serviceManager.specialCityCostService.GetAllSpecialCityCostAsync(pram);

        return Ok(specialCity);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SpecialCityCostDTO>> GetById(int id)
    {
        var speicalCity = await serviceManager.specialCityCostService.GetSpecialCityCostAsync(id);

        return Ok(speicalCity);
    }


    [HttpPost]
    public async Task<ActionResult<SpecialCityCost>> Add(SpecialCityCostDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid SpecialCityCost data");

        await serviceManager.specialCityCostService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = DTO.Id }, DTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SpecialCityCost>> Update(int id, SpecialCityCostDTO DTO)
    {
        if (DTO is null || id != DTO.Id)
            return BadRequest("Invalid SpecialCityCost data.");

        try
        {
            await serviceManager.specialCityCostService.UpdateAsync(DTO);

            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
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
