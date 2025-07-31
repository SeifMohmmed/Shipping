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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ShippingTypeDTO>> Get(int id)
    {
        var shippingtype = await _serviceManager.shippingTypeService.GetShippingTypeAsync(id);

        if (shippingtype is null)
            return NotFound($"Shipping Type with id {id} was not found.");


        return Ok(shippingtype);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ShippingTypeDTO>> Add(ShippingTypeAddDTO dto)
    {
        if (dto is null)
            return BadRequest("Invalid ShippingType data");

        await _serviceManager.shippingTypeService.AddAsync(dto);

        return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ShippingTypeDTO>> Update(int id, [FromBody] ShippingTypeUpdateDTO dto)
    {
        try
        {
            await _serviceManager.shippingTypeService.UpdateAsync(id, dto);
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
            await _serviceManager.shippingTypeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
