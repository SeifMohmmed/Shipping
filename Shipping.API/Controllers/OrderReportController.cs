using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderReportController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderReportToShowDTO>>> GetAll([FromQuery] OrderReportPramter pramter)
    {
        var AllOrderReport =
                await serviceManager.orderReportService.GetAllOrderReportAsync(pramter);

        return Ok(AllOrderReport);
    }



    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderReportDTO>> GetById(int id)
    {
        var OrderReport = await serviceManager.orderReportService.GetOrderReportAsync(id);

        if (OrderReport is null)
            return NotFound($"Order Report with id {id} was not found.");

        return Ok(OrderReport);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, [FromBody] OrderReportDTO DTO)
    {
        try
        {
            await serviceManager.orderReportService.UpdateAsync(id, DTO);
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
            await serviceManager.orderReportService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
