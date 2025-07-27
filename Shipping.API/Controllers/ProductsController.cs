using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] Pramter pramter)
    {
        var products = await _serviceManager.productService.GetProductsAsync(pramter);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await _serviceManager.productService.GetProductAsync(id);

        if (product is null)
            return NotFound($"Product with id {id} not found.");

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Add(ProductDTO dto)
    {
        if (dto is null)
            return BadRequest("Invalid Product data");


        await _serviceManager.productService.AddAsync(dto);

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDTO>> Update(int id, [FromBody] UpdateProductDTO dto)
    {
        if (dto is null || id != dto.Id)
            return BadRequest("Invalid Product data.");

        try
        {
            await _serviceManager.productService.UpdateAsync(dto);
            return NoContent(); // 204 No Content (successful update)
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        try
        {
            await _serviceManager.productService.DeleteAsync(id);
            return NoContent(); // 204 No Content (successful deletion)
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

}
