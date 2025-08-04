using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderReportController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewOrderReports)]
    public async Task<ActionResult<IEnumerable<OrderReportToShowDTO>>> GetAll([FromQuery] OrderReportPramter pramter)
    {
        var AllOrderReport =
                await serviceManager.orderReportService.GetAllOrderReportAsync(pramter);

        return Ok(AllOrderReport);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewOrderReports)]
    public async Task<ActionResult<OrderReportDTO>> GetById(int id)
    {
        var OrderReport = await serviceManager.orderReportService.GetOrderReportAsync(id);

        return Ok(OrderReport);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrderReports)]
    public async Task<ActionResult> Update(int id, [FromBody] OrderReportDTO DTO)
    {
        await serviceManager.orderReportService.UpdateAsync(id, DTO);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteOrderReports)]
    public async Task<ActionResult> Delete(int id)
    {
        await serviceManager.orderReportService.DeleteAsync(id);
        return NoContent();
    }
}
