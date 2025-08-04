using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ShippingTypeController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewSettings)]
    public async Task<ActionResult<IEnumerable<ShippingTypeDTO>>> GetAll([FromQuery] PaginationParameters pramter)
    {
        var shippingtypes = await serviceManager.shippingTypeService.GetAllShippingTypeAsync(pramter);

        return Ok(shippingtypes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewSettings)]
    public async Task<ActionResult<ShippingTypeDTO>> Get(int id)
    {
        var shippingtype = await serviceManager.shippingTypeService.GetShippingTypeAsync(id);

        return Ok(shippingtype);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [HasPermission(Permissions.AddSettings)]
    public async Task<ActionResult<ShippingTypeDTO>> Add(ShippingTypeAddDTO DTO)
    {
        await serviceManager.shippingTypeService.AddAsync(DTO);

        return CreatedAtAction(nameof(Get), new { id = DTO.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.UpdateSettings)]
    public async Task<ActionResult<ShippingTypeDTO>> Update(int id, [FromBody] ShippingTypeUpdateDTO DTO)
    {
        await serviceManager.shippingTypeService.UpdateAsync(id, DTO);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteSettings)]
    public async Task<ActionResult> Delete(int id)
    {
        await serviceManager.shippingTypeService.DeleteAsync(id);

        return NoContent();
    }
}
