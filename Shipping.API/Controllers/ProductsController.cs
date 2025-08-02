using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] PaginationParameters pramter)
    {
        var products = await _serviceManager.productService.GetProductsAsync(pramter);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await _serviceManager.productService.GetProductAsync(id);

        if (product is null)
            return NotFound($"Product with id {id} not found.");

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult<ProductDTO>> Add(ProductDTO dto)
    {
        var product = await _serviceManager.productService.AddAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] UpdateProductDTO dto)
    {
        try
        {
            await _serviceManager.productService.UpdateAsync(id, dto);
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
    [HasPermission(Permissions.DeleteOrders)]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        try
        {
            await _serviceManager.productService.DeleteAsync(id);
            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}
