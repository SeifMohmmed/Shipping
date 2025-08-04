using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] PaginationParameters pramter)
    {
        var products = await serviceManager.productService.GetProductsAsync(pramter);
        return Ok(products);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await serviceManager.productService.GetProductAsync(id);

        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult<ProductDTO>> Add(ProductDTO DTO)
    {
        var product = await serviceManager.productService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = DTO.Id }, DTO);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] UpdateProductDTO DTO)
    {
        await serviceManager.productService.UpdateAsync(id, DTO);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteOrders)]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        await serviceManager.productService.DeleteAsync(id);

        return NoContent();
    }

}
