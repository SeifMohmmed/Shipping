using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ShippingTypeController(IServiceManager _serviceManager) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShippingTypeDTO>>> GetAll([FromQuery] PaginationParameters pramter)
    {
        var shippingtypes = await _serviceManager.shippingTypeService.GetAllShippingTypeAsync(pramter);

        return Ok(shippingtypes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShippingTypeDTO>> Get(int id)
    {
        var shippingtype = await _serviceManager.shippingTypeService.GetShippingTypeAsync(id);

        return Ok(shippingtype);
    }

    [HttpPost]
    public async Task<ActionResult<ShippingTypeDTO>> Add(ShippingTypeDTO dto)
    {
        if (dto is null)
            return BadRequest("Invalid ShippingType data");

        await _serviceManager.shippingTypeService.AddAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShippingTypeDTO>> Update(int id, [FromBody] ShippingTypeDTO dto)
    {
        if (dto is null || id != dto.Id)
            return BadRequest("Invalid ShippingType data");
        try
        {
            await _serviceManager.shippingTypeService.UpdateAsync(dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _serviceManager.shippingTypeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
